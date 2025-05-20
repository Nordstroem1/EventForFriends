using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Comment
{
    public class CreateCommentDto
    {
        public string CommentContent { get; set; }
        public DateTime TimeSent { get; set; }
        [Required]
        public string EventId { get; set; }
        public CreateCommentDto(string commentId, string content, DateTime createdAt, string eventId)
        {
            CommentContent = content;
            TimeSent = createdAt;
            EventId = eventId;
        }
    }
}
