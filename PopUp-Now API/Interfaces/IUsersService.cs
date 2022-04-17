using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PopUp_Now_API.Model;
using PopUp_Now_API.Model.Requests;

namespace PopUp_Now_API.Interfaces
{
    public interface IUsersService
    {
        Task Delete(string username);
        Task<IdentityResult> RegisterUserAsync(RegisterRequest request);
        Task<string> LoginUserAsync(UserLogin request);
        Task<User> GetUser(string email);
        Task Update(User user, UserUpdateRequest userUpdateRequest);
        Task<string> ForgotPasswordAsync(string email);
        Task<string> ResetPasswordAsync(ResetPasswordRequest request);
        Task UpdateProfilePic(User user, Image image);
    }
}