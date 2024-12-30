using Domain.Models;

namespace Application.Dtos.User
{
    public class UpdateUserDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int PhoneNumber { get; set; }
        public RoleEnums.Roles Role { get; set; }
        public UpdateUserDto(Guid id, string userName, string email, string password, int phoneNumber, RoleEnums.Roles roleEnums)
        {
            UserName = userName;
            Email = email;
            Password = password;
            PhoneNumber = phoneNumber;
            Role = RoleEnums.Roles.user;
        }
    }
}
