using System.Net.Http.Headers;
using HR.LeaveManagement.MVC.Contracts;

namespace HR.LeaveManagement.MVC.Services.Base
{
    public class BaseHttpService
    {
        protected readonly ILocalStorageService _localStorage;
        protected IClient _client;
        public BaseHttpService(ILocalStorageService localStorage, IClient client)
        {
            _localStorage = localStorage;
            _client = client;
        }

        protected Response<Guid> ConvertApiExceptions<Guid>(ApiException exception)
        {
            if (exception.StatusCode == 400)
            {
                return new Response<Guid> { Message = "Validation errors have occurred.", ValidationErrors = exception.Response, Success = false };
            }

            if (exception.StatusCode == 404)
            {
                return new Response<Guid> { Message = "The requested item could not be found.", Success = false };
            }
            if (exception.StatusCode >= 200 && exception.StatusCode <= 299)
            {
                return new Response<Guid>() { Message = "Operation Reported Success", Success = true };
            }

            return new Response<Guid> { Message = "Something went wrong, please try again.", Success = false };
        }

        protected void AddBearerToken()
        {
            if (_localStorage.Exist("token"))
            {
                _client.HttpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _localStorage.GetStorageValue<string>("token"));
            }
        }
    }
}
