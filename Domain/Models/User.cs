using Microsoft.AspNetCore.Identity;

namespace Domain.Models
{
    public class User : IdentityUser
    {
        public DateTime CreatedAt { get; set; }
        public List<Event> Events { get; set; }
        public List<Comment> Comments{ get; set; }
        public string Role { get; set; }
        public string ProfilePicture { get; set; }
        public double Longitude { get; set; } 
        public double Latitude { get; set; } 

        public User(string userName, string email, int phoneNumber, string password, DateTime createdAt, string role, string profilePicture, double longitude, double latitude)
        {
            UserName = userName;
            Email = email;
            PhoneNumber = phoneNumber.ToString();
            CreatedAt = createdAt;
            Events = new List<Event>();
            Comments = new List<Comment>();
            LockoutEnabled = true;
            LockoutEnd = null;
            Role = role;
            ProfilePicture = profilePicture;
            Longitude = longitude;
            Latitude = latitude;
        }
        public User() { }
    }
}
