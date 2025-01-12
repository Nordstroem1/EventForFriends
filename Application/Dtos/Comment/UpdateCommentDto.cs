using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Comment
{
    public class UpdateCommentDto
    {
        [Required]
        public Guid EventId { get; set; }
        public Guid CommentId { get; set; }
        [Required]
        [MaxLength(400)]
        public string CommentContent { get; set; }
    }
}
