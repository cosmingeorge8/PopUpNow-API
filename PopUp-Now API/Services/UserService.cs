using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PopUp_Now_API.Interfaces;
using PopUp_Now_API.Model;
using PopUp_Now_API.Model.Requests;

namespace PopUp_Now_API.Services
{
    public class UserService : IUsersService
    {
        private readonly IMailService _mailService;

        private readonly UserManager<IdentityUser> _userManager;

        /**
         * Helper field used for reading  appsettings.json
         */
        private readonly IConfiguration _configuration;

        public UserService(
            UserManager<IdentityUser> userManager,
            IConfiguration configuration, IMailService mailService)
        {
            _userManager = userManager;
            _configuration = configuration;
            _mailService = mailService;
        }

        /**
         * delete user by username
         */
        public async Task Delete(string email)
        {
            var user = await GetUser(email);
            if (user is null)
            {
                throw new Exception("User not found");
            }

            await _userManager.DeleteAsync(user);
        }

        /**
         * Update user
         */
        public void Update(IdentityUser user)
        {
            _userManager.UpdateAsync(user);
        }

        public async Task<string> ForgotPasswordAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)
            {
                throw new Exception($"No user found with {email}");
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = Encoding.UTF8.GetBytes(token);
            var validToken = WebEncoders.Base64UrlEncode(encodedToken);

            var url = $"{_configuration["AppUrl"]}/ResetPassword?email={email}&token={validToken}";
            await _mailService.SendEmailAsync(new Email()
            {
                ToEmail = email,
                Subject = "Reset Password",
                Body = "<h1>Follow instructions to reset password</h1" +
                       $"<p>To reset your password <a href='{url}'>click here</a></p>",
            });

            return "Email was sent to user";
        }

        public async Task<string> ResetPasswordAsync(ResetPasswordRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                throw new Exception("User not found");
            }

            if (request.NewPassword != request.ConfirmPassword)
            {
                throw new Exception("Passwords do not match");
            }

            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);

            if (!result.Succeeded)
            {
                throw new Exception(result.Errors.ToString());
            }

            return "Password has been reset successfully";
        }

        public async Task<IdentityResult> RegisterUserAsync(RegisterRequest request)
        {
            if (request is null)
            {
                throw new Exception("RegisterRequest is null");
            }

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is not null)
            {
                throw new Exception("There is already an account with this e-mail");
            }

            if (request.Password != request.ConfirmPassword)
            {
                throw new Exception("Passwords do not match");
            }

            var result = await RegisterUser(request);

            if (!result.Succeeded)
            {
                throw new Exception(ErrorsToString(result));
            }

            return result;
        }

        private static string ErrorsToString(IdentityResult result)
        {
            var errorsToString = "";
            foreach (var error in result.Errors)
            {
                errorsToString += error.Description + "\n";
            }

            return errorsToString;
        }

        private async Task<IdentityResult> RegisterUser(RegisterRequest request)
        {
            var identityUser = GetUser(request);

            var result = await _userManager.CreateAsync(identityUser, request.Password);

            await AddToRole(identityUser);
            return result;
        }

        private async Task AddToRole(IdentityUser identityUser)
        {
            if (identityUser is Landlord)
            {
                await _userManager.AddToRoleAsync(identityUser, "Landlord");
            }
            else
            {
                await _userManager.AddToRoleAsync(identityUser, "User");
            }
        }

        protected virtual IdentityUser GetUser(RegisterRequest request)
        {
            if (request.LandlordRequest)
            {
                return new Landlord()
                {
                    Email = request.Email,
                    Name = request.Name,
                    UserName = request.GetUserName(),
                    Active = false
                };
            }

            return new User
            {
                Email = request.Email,
                Name = request.Name,
                UserName = request.GetUserName()
            };
        }

        public async Task<string> LoginUserAsync(UserLogin request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
            {
                throw new Exception("User not found");
            }

            var result = await _userManager.CheckPasswordAsync(user, request.Password);

            if (!result)
            {
                throw new Exception("Invalid password");
            }

            /*
             * Initialize claims
             */
            var claims = await GetClaims(user);


            /*
             * Get the JWT key
             */
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration["JWT:Secret"]));

            /*
             * Create token
             */
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:ValidIssuer"],
                audience: _configuration["Jwt:ValidAudience"],
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));
            /*
             * Write it
             */
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            /*
             * return it
             */
            return tokenString;
        }

        private async Task<List<Claim>> GetClaims(IdentityUser user)
        {
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.UserName),
                new(ClaimTypes.Email, user.Email),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var roleClaims = new List<Claim>();

            foreach (var role in await _userManager.GetRolesAsync(user))
            {
                roleClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            claims.AddRange(roleClaims);

            return claims;
        }

        public async Task<IdentityUser> GetUser(string email)
        {
            var result = await _userManager.FindByEmailAsync(email);
            if (result is null)
            {
                throw new Exception("User not found");
            }

            return result;
        }
    }
}