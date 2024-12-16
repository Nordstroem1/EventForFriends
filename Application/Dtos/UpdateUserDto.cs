using Domain.Models;

namespace Application.Dtos
{
    public class UpdateUserDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int PhoneNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public RoleEnums.Roles Role { get; set; }
        public UpdateUserDto(Guid id, string userName, string email, string password, int phoneNumber, DateTime createdAt, RoleEnums.Roles roleEnums)
        {
            UserName = userName;
            Email = email;
            Password = password;
            PhoneNumber = phoneNumber;
            CreatedAt = createdAt;
            Role = RoleEnums.Roles.user;
        }
    }
}
