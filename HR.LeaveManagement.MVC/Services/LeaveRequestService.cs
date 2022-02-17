using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HR.LeaveManagement.MVC.Contracts;
using HR.LeaveManagement.MVC.Models.LeaveAllocations;
using HR.LeaveManagement.MVC.Models.LeaveRequests;
using HR.LeaveManagement.MVC.Services.Base;

namespace HR.LeaveManagement.MVC.Services
{
    public class LeaveRequestService : BaseHttpService, ILeaveRequestService
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly IMapper _mapper;
        private readonly IClient _httpClient;

        public LeaveRequestService(IMapper mapper,
            ILocalStorageService localStorageService,
            IClient httpClient) : base(localStorageService, httpClient)
        {
            _localStorageService = localStorageService;
            _mapper = mapper;
            _httpClient = httpClient;
        }

        public async Task<AdminLeaveRequestViewModel> GetAdminLeaveRequestList()
        {
            AddBearerToken();
            var leaveRequests = await _httpClient.LeaveRequestsAllAsync(false);

            var model = new AdminLeaveRequestViewModel
            {
                TotalRequests = leaveRequests.Count,
                ApprovedRequests = leaveRequests.Count(x => x.Approved == true),
                PendingRequests = leaveRequests.Count(x => x.Approved == null),
                RejectedRequests = leaveRequests.Count(x => x.Approved == false),
                LeaveRequests = _mapper.Map<List<LeaveRequestViewModel>>(leaveRequests)
            };

            return model;
        }

        public async Task<EmployeeLeaveRequestViewViewModel> GetUserLeaveRequest()
        {
            AddBearerToken();
            var leaveRequests = await _httpClient.LeaveRequestsAllAsync(true);
            var allocations = await _httpClient.LeaveAllocationsAllAsync(true);

            var model = new EmployeeLeaveRequestViewViewModel
            {
                LeaveAllocations = _mapper.Map<List<LeaveAllocationViewModel>>(allocations),
                LeaveRequests = _mapper.Map<List<LeaveRequestViewModel>>(leaveRequests)
            };

            return model;
        }

        public async Task<Response<int>> CreateLeaveRequest(CreateLeaveRequestViewModel leaveRequestViewModel)
        {
            try
            {
                var response = new Response<int>();
                var createLeaveRequest = _mapper.Map<CreateLeaveRequestDto>(leaveRequestViewModel);
                AddBearerToken();

                var apiResponse = await _httpClient.LeaveRequestsPOSTAsync(createLeaveRequest);
                if (apiResponse.Success)
                {
                    response.Data = apiResponse.Id;
                    response.Success = true;
                }
                else
                {
                    foreach (var error in apiResponse.Errors)
                    {
                        response.ValidationErrors += error + Environment.NewLine;
                    }
                }

                return response;
            }
            catch (ApiException ex)
            {
                return ConvertApiExceptions<int>(ex);
            }
        }

        public async Task<LeaveRequestViewModel> GetLeaveRequest(int id)
        {
            AddBearerToken();
            var leaveRequest = await _httpClient.LeaveRequestsGETAsync(id);
            return _mapper.Map<LeaveRequestViewModel>(leaveRequest);
        }

        public Task DeleteLeaveRequest(int id)
        {
            throw new NotImplementedException();
        }

        public async Task ApproveLeaveRequest(int id, bool approved)
        {
            AddBearerToken();
            try
            {
                var request = new ChangeLeaveRequestApprovalDto { Approved = approved, Id = id };
                await _httpClient.ChangeapprovalAsync(id, request);
            }
            catch (Exception)
            {
                // ignored
            }
        }
    }
}
