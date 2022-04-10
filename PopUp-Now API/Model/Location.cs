namespace PopUp_Now_API.Model
{
    public class Location
    {
        public int Id { get; set; }

        public string Street { get; set; }

        public string Number { get; set; }

        public string City { get; set; }

        public string Country { get; set; }

        public string Postal { get; set; }

        public override string ToString()
        {
            return Street + Number + City + City + Postal;
        }
    }
}