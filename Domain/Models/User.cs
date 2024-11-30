using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class User : IdentityUser<Guid>
    {
        public DateTime CreatedAt { get; set; }
        public List<Event> Events { get; set; }
        public List<Message> Messages { get; set; }

        public User()
        {
            Events = new List<Event>();
            Messages = new List<Message>();
        }
        public User(Guid userId, string userName, string email, int phoneNumber, string password, DateTime createdAt, List<Event>? events, List<Message>? messages)
        {
            Id = userId;
            UserName = userName;
            Email = email;
            PhoneNumber = phoneNumber.ToString();
            PasswordHash = password; 
            CreatedAt = createdAt;
            Events = events;
            Messages = messages;
        }
    }
}
