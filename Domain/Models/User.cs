using Microsoft.AspNetCore.Identity;

namespace Domain.Models
{
    public class User : IdentityUser
    {
        public DateTime CreatedAt { get; set; }
        public List<Event> Events { get; set; }
        public List<Comment> Comments{ get; set; }
        public string Role { get; set; }

        public User(string userName, string email, int phoneNumber, string password, DateTime createdAt, string role)
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
        }
        public User() { }
    }
}
