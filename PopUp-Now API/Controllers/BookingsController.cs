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
public class BookingsController : ControllerBase
{
    private readonly IBookingService _bookingService;
    private readonly IUsersService _userService;

    public BookingsController(IBookingService bookingService, IUsersService userService)
    {
        _userService = userService;
        _bookingService = bookingService;
    }

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
            var booking = await _bookingService.BookProperty(bookingRequest);
            return Created(booking.GetURl(), booking);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [Authorize(Roles = "Landlord")]
    [Authorize]
    [HttpPost("{bookingId:int}")]
    public IActionResult ConfirmBooking(int bookingId)
    {
        try
        {
            _bookingService.ConfirmBooking(bookingId);
            return Ok("Booking was confirmed");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
        
    }

    [Authorize(Roles = "Landlord")]
    [Authorize]
    [HttpGet("{propertyId:int}")]
    public async Task<IActionResult> GetBookings(int propertyId)
    {
        try
        {
            var landlord = (Landlord) await _userService.GetUser(User.FindFirst(ClaimTypes.Email)?.Value!);
            var bookings = landlord.Properties;
            return Ok(bookings);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }

    }
}
}
