using Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.User
{
    public class ChangeRoleDto
    {
        [Required]
        public Guid UserId { get; set; }
        [Required]
        public RoleEnums.Roles Role { get; set; }

        public ChangeRoleDto(Guid userId, RoleEnums.Roles role)
        {
            UserId = userId;
            Role = role;
        }
    }
}
