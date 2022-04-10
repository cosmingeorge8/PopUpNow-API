using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PopUp_Now_API.Model.Requests;

namespace PopUp_Now_API.Interfaces
{
    public interface IUsersService
    {

        Task Delete(string username);

        Task<IdentityResult> RegisterUserAsync(RegisterRequest request);

        Task<string> LoginUserAsync(UserLogin request);
        Task<IdentityUser> GetUser(string email);
        void Update(IdentityUser user);

        Task<string> ForgotPasswordAsync(string email);

        Task<string> ResetPasswordAsync(ResetPasswordRequest request);
    }
}

