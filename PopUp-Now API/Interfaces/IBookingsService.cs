using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using PopUp_Now_API.Model;
using PopUp_Now_API.Model.Requests;

namespace PopUp_Now_API.Interfaces
{
    public interface IBookingService 
    {
        Task<Booking> BookProperty(IdentityUser user, BookingRequest bookingRequest);
        Task<Booking> Get(int bookingId);
        Task ConfirmBooking(int bookingId);
    }
}

