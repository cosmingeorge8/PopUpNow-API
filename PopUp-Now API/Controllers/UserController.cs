using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PopUp_Now_API.Interfaces;
using PopUp_Now_API.Model;
using PopUp_Now_API.Model.Requests;

namespace PopUp_Now_API.Controllers
{
    /**
 * @author Mucalau Cosmin
 */
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUsersService _userRepository;

        /**
     * Constructor gets called by the framework, used to initialize the IConfiguration field.
     */
        public UserController(IUsersService userRepository)
        {
            _userRepository = userRepository;
        }

        /**
     * Get User
     */
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            var user = await _userRepository.GetUser(email);

            if (user is null)
            {
                return BadRequest("User is not authenticated");
            }

            try
            {
                user = await _userRepository.GetUser(user.Email);
                return Ok(user);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /**
     * Register
     */
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Properties not valid");
            }

            try
            {
                var result = await _userRepository.RegisterUserAsync(request);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /**
     * Login
     */
        [HttpPost("Login")]
        public async Task<IActionResult> Login(UserLogin userLogin)
        {
            try
            {
                var result = await _userRepository.LoginUserAsync(userLogin);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /**
     * Delete user
     *
     */
        [Authorize(Roles = "User,Landlord")]
        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete()
        {
            try
            {
                var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                await _userRepository.Delete(username!);
                return Ok();
            }
            catch (Exception e)
            {
                return NotFound(e);
            }
        }

        /**
     * Update user
     */
        [Authorize(Roles = "User,Landlord")]
        [HttpPatch("Update")]
        public IActionResult Update(User user)
        {
            _userRepository.Update(user);
            return Ok();
        }

        /**
     * Forgot password
     */
        [HttpPost("ForgetPassword")]
        public async Task<IActionResult> ForgetPassword(string email)
        {
            if (email is null || email.Equals(" "))
            {
                return BadRequest("Email is null");
            }

            try
            {
                var result = await _userRepository.ForgotPasswordAsync(email);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /**
     * Reset password
     */
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }

            try
            {
                var result = await _userRepository.ResetPasswordAsync(request);
                return Ok(result);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /**
         * Set profile pic
         */
        [Route("ProfilePicture")]
        [Authorize(Roles = "User,Landlord")]
        [HttpPatch]
        public async Task<IActionResult> UpdateProfilePic(Image image)
        {
            try
            {
                var user = await _userRepository.GetUser(User.FindFirst(ClaimTypes.Email)?.Value);
                await _userRepository.UpdateProfilePic(user,image);
                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}