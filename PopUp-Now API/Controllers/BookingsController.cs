using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PopUp_Now_API.Interfaces;
using PopUp_Now_API.Model.Requests;

namespace PopUp_Now_API.Controllers
{
    /**
 * @author Mucalau Cosmin
 */
    [ApiController]
    [Route("[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly IUsersService _userService;

        public BookingsController(IBookingService bookingService, IUsersService userService)
        {
            _userService = userService;
            _bookingService = bookingService;
        }

        /**
         * Get the list of bookings for a logged in user
         */
        [Authorize(Roles = "User,Landlord")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var user = await _userService.GetUser(User.FindFirst(ClaimTypes.Email).Value);
            var bookings = await _bookingService.GetAll(user);
            return bookings.Any() ? Ok(bookings) : NotFound();
        }

        /**
         * Get a list of all booking requests for a landlord
         * Method is accessible for Landlords only
         */
        [Route("requests")]
        [Authorize(Roles = "Landlord")]
        [HttpGet]
        public async Task<IActionResult> GetBookingRequests()
        {
            var user = await _userService.GetUser(User.FindFirst(ClaimTypes.Email).Value);
            var bookings = await _bookingService.GetBookingRequests(user);
            return bookings.Any() ? Ok(bookings) : NotFound();
        }

        /**
         * Book a property
         * Method takes in a bookingRequests which is processed in the booking service
         */
        [Authorize(Roles = "User,Landlord")]
        [HttpPost]
        public async Task<IActionResult> BookProperty(BookingRequest bookingRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }

            try
            {
                var user = await _userService.GetUser(User.FindFirst(ClaimTypes.Email).Value);
                var booking = await _bookingService.BookProperty(user, bookingRequest);
                return Created(booking.GetURl(), booking);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        /**
         * Confirm or decline a booking as a landlord
         */
        [Authorize(Roles = "Landlord")]
        [HttpPut]
        public async Task<IActionResult> ConfirmBooking(BookingConfirmationRequest bookingConfirmationRequest)
        {
            try
            {
                await _bookingService.ConfirmBooking(bookingConfirmationRequest.BookingId,
                    bookingConfirmationRequest.BookingStatus);
                return Ok(bookingConfirmationRequest.BookingStatus);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}