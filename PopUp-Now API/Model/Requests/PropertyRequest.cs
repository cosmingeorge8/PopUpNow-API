using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace PopUp_Now_API.Model.Requests
{
    public class PropertyRequest
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")] public string Name { get; set; }

        [Required(ErrorMessage = "description is required")] public string Description { get; set; }

        [Required(ErrorMessage = "Location is required")] public Location Location { get; set; }

        [Required(ErrorMessage = "price is required")] public Price Price { get; set; }

        [Required(ErrorMessage = "category is required")] public Category Category { get; set; }

        [Required(ErrorMessage = "size is required")] public int Size { get; set; }
        
        [Required(ErrorMessage = "minimumBookingDays is required")] public int MinimumBookingDays { get; set; }

        public Image Image { get; set; }

        public Collection<Image> Images { get; set; }
    }
}