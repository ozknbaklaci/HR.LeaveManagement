using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using HR.LeaveManagement.MVC.Contracts;
using HR.LeaveManagement.MVC.Services.Base;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;

namespace HR.LeaveManagement.MVC.CustomMiddleware
{
    public class RequestMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILocalStorageService _localStorageService;

        public RequestMiddleware(RequestDelegate next,
            ILocalStorageService localStorageService)
        {
            _next = next;
            _localStorageService = localStorageService;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                var endPoint = httpContext.Features.Get<IEndpointFeature>()?.Endpoint;
                var authAttr = endPoint?.Metadata.GetMetadata<AuthorizeAttribute>();
                if (authAttr != null)
                {
                    var tokenExists = _localStorageService.Exist("token");
                    var tokenIsValid = true;

                    if (tokenExists)
                    {
                        var token = _localStorageService.GetStorageValue<string>("token");
                        var tokenHandler = new JwtSecurityTokenHandler();

                        var tokenContent = tokenHandler.ReadJwtToken(token);
                        var expiry = tokenContent.ValidTo;

                        if (expiry < DateTime.UtcNow)
                        {
                            tokenIsValid = false;
                        }
                    }

                    if (!tokenIsValid || !tokenExists)
                    {
                        await SingOutAndRedirect(httpContext);
                        return;
                    }

                    if (authAttr.Roles != null)
                    {
                        var userRole = httpContext.User.FindFirst(ClaimTypes.Role)?.Value;
                        if (authAttr.Roles.Contains(userRole) == false)
                        {
                            var path = "/home/notauthorized";
                            httpContext.Response.Redirect(path);
                            return;
                        }
                    }
                }

                await _next(httpContext);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(httpContext, exception);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
        {
            switch (exception)
            {
                case ApiException:
                    await SingOutAndRedirect(httpContext);
                    break;
                default:
                    var path = "/Home/Error/";
                    httpContext.Response.Redirect(path);
                    break;
            }
        }

        private static async Task SingOutAndRedirect(HttpContext httpContext)
        {
            await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var path = "/users/login/";
            httpContext.Response.Redirect(path);
        }
    }
}
