namespace MentalHealth.Models.UserAccount
{
    public class Register
    {
        public string Surname { get; set; }
        public string FirstName { get; set; }
        public string OtherNames { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string IdNo { get; set; }
        public double LocationLatitude { get; set; }
        public double LocationLongitude { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
