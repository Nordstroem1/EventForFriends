using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Comment
{
    public class CreateCommentDto
    {
        public string CommentContent { get; set; }
        public DateTime TimeSent { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public Guid EventId { get; set; }
        public CreateCommentDto(Guid commentId, string content, DateTime createdAt, Guid userId, Guid eventId)
        {
            CommentContent = content;
            TimeSent = createdAt;
            UserId = userId;
            EventId = eventId;
        }
    }
}
