using System.ComponentModel.DataAnnotations;

namespace PopUp_Now_API.Model.Requests
{
    public class BookingConfirmationRequest
    {
        [Required] public int BookingId { get; set; }
        [Required] public BookingStatus BookingStatus { get; set; }
    }
}