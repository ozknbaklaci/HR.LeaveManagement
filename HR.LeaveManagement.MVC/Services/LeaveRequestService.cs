using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HR.LeaveManagement.MVC.Contracts;
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

        public Task<AdminLeaveRequestViewModel> GetAdminLeaveRequestList()
        {
            throw new NotImplementedException();
        }

        public Task<EmployeeLeaveRequestViewViewModel> GetUserLeaveRequest()
        {
            throw new NotImplementedException();
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

        public Task DeleteLeaveRequest(int id)
        {
            throw new NotImplementedException();
        }


    }
}
