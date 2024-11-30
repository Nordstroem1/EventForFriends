using Domain.Models;

namespace Application.Dtos
{
    public class CreateUserDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int PhoneNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsAdmin { get; set; }
        public List<Event> Events { get; set; }
        public List<Message> Messages { get; set; }
        public CreateUserDto(string userName, string email, string password, int phoneNumber, DateTime createdAt, bool isAdmin, List<Event> events, List<Message>? messages)
        {
            UserName = userName;
            Email = email;
            Password = password;
            PhoneNumber = phoneNumber;
            CreatedAt = createdAt;
            IsAdmin = isAdmin;
            Events = events;
            Messages = messages;
        }
    }
}
