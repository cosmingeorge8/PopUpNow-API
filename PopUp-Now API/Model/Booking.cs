using System;

namespace PopUp_Now_API.Model
{
    public class Booking
    {
        public int Id { get; set; }

        public User User { get; set; }

        public Property Property { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Reasoning { get; set; }

        public string SpecialRequests { get; set; }

        public bool Confirmed { get; set; }

        public string GetURl()
        {
            return $"/api/bookings/{Id}";
        }
    }
}