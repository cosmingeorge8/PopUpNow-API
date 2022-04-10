using System.Threading.Tasks;
using PopUp_Now_API.Model;
using PopUp_Now_API.Model.Requests;

namespace PopUp_Now_API.Interfaces
{
    public interface IBookingService 
    {
        Task<Booking> BookProperty(BookingRequest bookingRequest);
        Task<Booking> Get(int bookingId);
        Task ConfirmBooking(int bookingId);
    }
}

