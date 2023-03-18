namespace ProfileSimulator.Api.Data
{
    public class Profile
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Email { get; set; }
        public string Occupation { get; internal set; }
        public string? ProfileImageUrl { get; internal set; }
    }
}
