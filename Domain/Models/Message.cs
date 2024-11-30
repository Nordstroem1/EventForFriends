using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Message
    {
        [Required]
        public Guid MessageId { get; set; }
        [Required]
        [MaxLength(700)]
        public string MessageContent { get; set; }
        public DateTime TimeSent { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public Guid EventId { get; set; }
        public Message() { }
        public Message(Guid messageId, string content, DateTime createdAt, Guid userId, Guid eventId)
        {
            MessageId = messageId;
            MessageContent = content;
            TimeSent = createdAt;
            UserId = userId;
            EventId = eventId;
        }
}
}