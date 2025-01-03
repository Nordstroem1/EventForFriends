using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Comment
{
    public class UpdateCommentDto
    {
        public Guid CommentId { get; set; }
        [Required]
        [MaxLength(400)]
        public string CommentContent { get; set; }
    }
}
