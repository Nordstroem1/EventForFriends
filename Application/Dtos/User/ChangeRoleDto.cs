using Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.User
{
    public class ChangeRoleDto
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public string Role { get; set; }

        public ChangeRoleDto(Guid userId, string role)
        {
            UserId = userId;
            Role = role;
        }
    }
}
