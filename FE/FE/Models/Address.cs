namespace FE.Models
{
    public class Address
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string HouseNumber { get; set; }
        public string Street { get; set; }
        public string Ward { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}
