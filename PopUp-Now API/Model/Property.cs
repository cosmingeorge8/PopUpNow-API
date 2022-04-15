using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using PopUp_Now_API.Model.Requests;

namespace PopUp_Now_API.Model
{
    public class Property
    {
        public int Id { get; set; }

        public Collection<Image> detailImages { get; set; }

        public Image Image { get; set; }

        public Location location { get; set; }

        public int MinimumBookingDays { get; set; }

        public Price Price { get; set; }
        public string Name { get; set; }

        public string Description { get; set; }

        public Category Category { get; set; }

        public int Size { get; set; }
        public User User { get; set; }

        public string GetURl()
        {
            return $"/api/properties/{Id}";
        }

        public void Update(PropertyRequest propertyRequest)
        {
            Name = propertyRequest.Name;
            Description = propertyRequest.Description;
            Price = propertyRequest.Price;
            Category = propertyRequest.Category;
            Size = propertyRequest.Size;
        }
    }
}