using BusinessObject.Entities;

namespace DataTransfer.AddressDTOs
{
    public class AddressDTO
    {
        public string HouseNumber { get; set; }
        public string Street { get; set; }
        public string WardName { get; set; }
        public string DistrictName{ get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
    }
}
