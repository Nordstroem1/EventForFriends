using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Comment
    {
        [Required]
        public Guid CommentId { get; set; }
        [Required]
        [MaxLength(400)]
        public string CommentContent { get; set; }
        public DateTime TimeSent { get; set; }
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public Guid EventId { get; set; }
        public int Likes { get; set; } = 0;
        public Comment() { }
        public Comment(Guid commentId, string content, DateTime createdAt, Guid userId, Guid eventId, int likes)
        {
            CommentId = commentId;
            CommentContent = content;
            TimeSent = createdAt;
            UserId = userId;
            EventId = eventId;
            Likes = likes;
        }
    }
}