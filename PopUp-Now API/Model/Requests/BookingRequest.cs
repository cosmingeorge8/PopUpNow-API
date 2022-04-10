using System;
using System.ComponentModel.DataAnnotations;

namespace PopUp_Now_API.Model.Requests
{
    public class BookingRequest
    {
        [Required] public int PropertyId { get; set; }

        [Required] public DateTime StartDate { get; set; }

        [Required] public DateTime EndDate { get; set; }

        [Required]
        [StringLength(256, MinimumLength = 5)]
        public string Reasoning { get; set; }

        public string SpecialRequests { get; set; }
    }
}