using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HR.LeaveManagement.MVC.Contracts;
using HR.LeaveManagement.MVC.Models;
using HR.LeaveManagement.MVC.Models.Identity;

namespace HR.LeaveManagement.MVC.Controllers
{
    public class UsersController : Controller
    {
        private readonly IAuthenticationService _authenticationService;

        public UsersController(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public IActionResult Login(string returnUrl)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                returnUrl ??= Url.Content("~/");
                var isLoggedIn = await _authenticationService.Authenticate(model.Email, model.Password);

                if (isLoggedIn)
                {
                    return LocalRedirect(returnUrl);
                }
            }

            ModelState.AddModelError("", "Log in Attempt Failed. Please try again");
            return View(model);
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var returnUrl = Url.Content("~/");
                var isCreated = await _authenticationService.Register(model);
                if (isCreated)
                    return LocalRedirect(returnUrl);
            }

            ModelState.AddModelError("", "Registration Attempt Failed. Please try again.");
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout(string returnUrl)
        {
            returnUrl ??= Url.Content("~/");
            await _authenticationService.Logout();
            return LocalRedirect(returnUrl);
        }
    }
}
