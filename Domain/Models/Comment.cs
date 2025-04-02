using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Comment
    {
        [Required]
        public string CommentId { get; set; }
        [Required]
        [MaxLength(400)]
        public string CommentContent { get; set; }
        public DateTime TimeSent { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public string EventId { get; set; }
        public int Likes { get; set; } = 0;
        public Comment() { }
        public Comment(string commentId, string content, DateTime createdAt, string userId, string eventId, int likes)
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