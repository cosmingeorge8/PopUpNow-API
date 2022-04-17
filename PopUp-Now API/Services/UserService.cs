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
using static System.String;

namespace PopUp_Now_API.Services
{
    public class UserService : IUsersService
    {
        private readonly IMailService _mailService;

        private readonly UserManager<User> _userManager;

        /**
         * Helper field used for reading  appsettings.json
         */
        private readonly IConfiguration _configuration;


        public UserService(
            UserManager<User> userManager,
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
        public async Task Update(User user, UserUpdateRequest userUpdateRequest)
        {
            if (!IsNullOrEmpty(userUpdateRequest.Password) && !IsNullOrEmpty(userUpdateRequest.ConfirmPassword))
            {
                await ChangePassword(user, userUpdateRequest);
            }
        }

        /**
         * Change user password using the UserManager class
         * first assert that passwords are matching
         */
        private async Task ChangePassword(User user, UserUpdateRequest userUpdateRequest)
        {
            if (userUpdateRequest.ConfirmPassword != userUpdateRequest.Password)
            {
                throw new Exception("Passwords do not match");
            }

            var result =
                await _userManager.ChangePasswordAsync(user, userUpdateRequest.CurrentPassword,
                    userUpdateRequest.Password);
            if (!result.Succeeded)
            {
                throw new Exception(ErrorsToString(result));
            }
        }

        /**
         * Sends an email using SendGrip API containing an validation token to a given email address
         */
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

        /**
         * Resets a user's password
         * Validation done using the token received via e-mail
         */
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

        /**
         * Update user profile picture path
         */
        public async Task UpdateProfilePic(User user, Image image)
        {
            if (image.Path is null)
            {
                throw new Exception("Image has a null path");
            }

            user.Image = image.Path;
            await _userManager.UpdateAsync(user);
        }

        /**
         * Register user
         */
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

        /**
         * Helper method that takes an identity result and pretty formats the errors into string
         */
        private static string ErrorsToString(IdentityResult result)
        {
            var errorsToString = "";
            foreach (var error in result.Errors)
            {
                errorsToString += error.Description + "\n";
            }

            return errorsToString;
        }

        /**
         * Register user and add to specific role
         */
        private async Task<IdentityResult> RegisterUser(RegisterRequest request)
        {
            var User = GetUser(request);

            var result = await _userManager.CreateAsync(User, request.Password);

            if (result.Succeeded)
            {
                await AddToRole(User);
            }

            return result;
        }

        /**
         * Add a given user to role
         */
        private async Task AddToRole(User User)
        {
            if (User is Landlord)
            {
                await _userManager.AddToRoleAsync(User, "Landlord");
            }
            else
            {
                await _userManager.AddToRoleAsync(User, "User");
            }
        }

        /**
         * Determines whether the current user should be a regular user or a Landlord
         */
        protected virtual User GetUser(RegisterRequest request)
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

        /*
         * Login user
         * take in a UserLogin request and return a JWT 
         */
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

        /**
         * Initialize user claims
         */
        private async Task<List<Claim>> GetClaims(User user)
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

        /**
         * Get user by email
         */
        public async Task<User> GetUser(string email)
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