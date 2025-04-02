using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Comment
{
    public class CreateCommentDto
    {
        public string CommentContent { get; set; }
        public DateTime TimeSent { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string EventId { get; set; }
        public CreateCommentDto(string commentId, string content, DateTime createdAt, string userId, string eventId)
        {
            CommentContent = content;
            TimeSent = createdAt;
            UserId = userId;
            EventId = eventId;
        }
    }
}
