namespace Domain.Models
{
    public class User
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int PhoneNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<Event> Events { get; set; }
        public List<Message>? Messages { get; set; }
        public User(Guid userId, string userName, string email,int phoneNumber, string password, DateTime createdAt, List<Event> events, List<Message>? messages)
        {
            UserId = userId;
            UserName = userName;
            Email = email;
            PhoneNumber = phoneNumber;
            Password = password;
            CreatedAt = createdAt;
            Events = events;
            Messages = messages;
        }
    }
}
