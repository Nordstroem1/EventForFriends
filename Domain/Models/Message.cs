namespace Domain.Models
{
    public class Message
    {
        public Guid MessageId { get; set; }
        public string MessageContent { get; set; }
        public DateTime TimeSent { get; set; }
        public Guid UserId { get; set; }
        public Guid EventId { get; set; }
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