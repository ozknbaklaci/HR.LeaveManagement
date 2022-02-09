using System;
using System.Threading.Tasks;
using HR.LeaveManagement.MVC.Contracts;
using HR.LeaveManagement.MVC.Services.Base;

namespace HR.LeaveManagement.MVC.Services
{
    public class LeaveAllocationService : BaseHttpService, ILeaveAllocationService
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly IClient _client;
        public LeaveAllocationService(ILocalStorageService localStorage, IClient client, ILocalStorageService localStorageService)
            : base(localStorage, client)
        {
            _client = client;
            _localStorageService = localStorageService;
        }

        public async Task<Response<int>> CreateLeaveAllocations(int leaveTypeId)
        {
            try
            {
                var response = new Response<int>();
                var createLeaveAllocation = new CreateLeaveAllocationDto { LeaveTypeId = leaveTypeId };
                AddBearerToken();

                var apiResponse = await _client.LeaveAllocationsPOSTAsync(createLeaveAllocation);
                if (apiResponse.Success)
                {
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
    }
}
