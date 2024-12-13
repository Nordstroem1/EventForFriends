using Application.Dtos;
using Domain.Models;
using MediatR;

namespace Application.Queries.Login
{
    public class LoginCommand : IRequest<OperationResult<string>>
    {
        public LoginCommand(LoginDto loginDto) 
        {
            LoginDto = loginDto;   
        }
        public LoginDto LoginDto { get; set; }
    }
}
