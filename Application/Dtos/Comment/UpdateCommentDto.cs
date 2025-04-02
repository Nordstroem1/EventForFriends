using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Comment
{
    public class UpdateCommentDto
    {
        [Required]
        public string EventId { get; set; }
        public string CommentId { get; set; }
        [Required]
        [MaxLength(400)]
        public string CommentContent { get; set; }
    }
}
