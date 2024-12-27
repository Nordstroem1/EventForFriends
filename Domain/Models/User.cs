using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class User : IdentityUser<Guid>
    {
        public DateTime CreatedAt { get; set; }
        public List<Event> Events { get; set; }
        public List<Message> Messages { get; set; }
        [NotMapped]
        [Required(ErrorMessage = "Role is required")]
        public RoleEnums.Roles Role { get; set; }

        public User(Guid userId, string userName, string email, int phoneNumber, string password, DateTime createdAt, RoleEnums.Roles role, List<Event>? events, List<Message>? messages)
        {
            Id = userId;
            UserName = userName;
            Email = email;
            PhoneNumber = phoneNumber.ToString();
            PasswordHash = password; 
            CreatedAt = createdAt;
            Events = events;
            Messages = messages;
            LockoutEnabled = true;
            LockoutEnd = null;
            Role = role;
            Events = new List<Event>();
            Messages = new List<Message>();
        }
    }
}
