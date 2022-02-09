using System.Threading.Tasks;
using HR.LeaveManagement.MVC.Models;
using HR.LeaveManagement.MVC.Models.Identity;

namespace HR.LeaveManagement.MVC.Contracts
{
    public interface IAuthenticationService
    {
        Task<bool> Authenticate(string email, string password);
        Task<bool> Register(RegisterViewModel model);
        Task Logout();
    }
}
