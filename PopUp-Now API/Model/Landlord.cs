using System.Collections.ObjectModel;

namespace PopUp_Now_API.Model
{
    public class Landlord : User
    {
        public Collection<Property> Properties { get; set; }
        public bool Active { get; set; }

        public void AddProperty(Property property)
        {
            Properties.Add(property);
        }
    }
}