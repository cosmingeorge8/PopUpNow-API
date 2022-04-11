using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace PopUp_Now_API.Model.Requests
{
    public class PropertyRequest
    {
        public int Id { get; set; }
        [Required] public string Name { get; set; }

        [Required] public string Description { get; set; }

        [Required] public string Location { get; set; }

        [Required] public Price Price { get; set; }

        [Required] public Category Category { get; set; }

        [Required] public int Size { get; set; }
        
        [Required] public int MinimumBookingDays { get; set; }

        public string Image { get; set; }

        public Collection<string> Images { get; set; }

        public Location GetLocation()
        {
            var tokens = Location.Split(" ");

            return new Location
            {
                City = tokens[0],
                Country = tokens[1],
                Number = tokens[2],
                Postal = tokens[3],
                Street = tokens[4]
            };
        }
    }
}