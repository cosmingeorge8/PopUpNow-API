using System.Collections.Generic;
using System.Threading.Tasks;
using PopUp_Now_API.Model;
using PopUp_Now_API.Model.Requests;

namespace PopUp_Now_API.Interfaces
{
    public interface IBookingService 
    {
        Task<Booking> BookProperty(User user, BookingRequest bookingRequest);
        Task ConfirmBooking(int bookingId, BookingStatus status);
        Task<List<Booking>> GetAll(User user);
        Task<List<Booking>> GetBookingRequests(User user);
    }
}

