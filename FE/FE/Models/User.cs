namespace FE.Models
{
    public class User
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public byte Status { get; set; }
        public int Point { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
    }
}
