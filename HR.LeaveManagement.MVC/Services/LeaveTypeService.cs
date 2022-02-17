using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using HR.LeaveManagement.MVC.Contracts;
using HR.LeaveManagement.MVC.Models;
using HR.LeaveManagement.MVC.Models.LeaveTypes;
using HR.LeaveManagement.MVC.Services.Base;

namespace HR.LeaveManagement.MVC.Services
{
    public class LeaveTypeService : BaseHttpService, ILeaveTypeService
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly IMapper _mapper;
        private readonly IClient _httpClient;
        public LeaveTypeService(IMapper mapper,
            ILocalStorageService localStorageService,
            IClient httpClient) : base(localStorageService, httpClient)
        {
            _localStorageService = localStorageService;
            _mapper = mapper;
            _httpClient = httpClient;
        }

        public async Task<List<LeaveTypeViewModel>> GetLeaveTypes()
        {
            AddBearerToken();
            var leaveTypes = await _httpClient.LeaveTypesAllAsync();

            return _mapper.Map<List<LeaveTypeViewModel>>(leaveTypes);
        }

        public async Task<LeaveTypeViewModel> GetLeaveTypeDetails(int id)
        {
            AddBearerToken();
            var leaveType = await _httpClient.LeaveTypesGETAsync(id);

            return _mapper.Map<LeaveTypeViewModel>(leaveType);
        }

        public async Task<Response<int>> CreateLeaveType(CreateLeaveTypeViewModel leaveTypeViewModel)
        {
            try
            {
                var response = new Response<int>();
                var createLeaveType = _mapper.Map<CreateLeaveTypeDto>(leaveTypeViewModel);
                AddBearerToken();
                var apiResponse = await _httpClient.LeaveTypesPOSTAsync(createLeaveType);
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
            catch (ApiException exception)
            {
                return ConvertApiExceptions<int>(exception);
            }
        }

        public async Task<Response<int>> UpdateLeaveType(int id, LeaveTypeViewModel leaveTypeViewModel)
        {
            try
            {
                var leaveTypeDto = _mapper.Map<LeaveTypeDto>(leaveTypeViewModel);
                AddBearerToken();
                await _httpClient.LeaveTypesPUTAsync(id.ToString(), leaveTypeDto);
                return new Response<int>() { Success = true };
            }
            catch (ApiException ex)
            {
                return ConvertApiExceptions<int>(ex);
            }
        }

        public async Task<Response<int>> DeleteLeaveType(int id)
        {
            try
            {
                AddBearerToken();
                await _httpClient.LeaveTypesDELETEAsync(id);

                return new Response<int> { Success = true };
            }
            catch (ApiException exception)
            {
                return ConvertApiExceptions<int>(exception);
            }
        }
    }
}
