using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Queries.GetUserById
{
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, OperationResult<User>>
    {
        private readonly UserManager<User> _userManager;
        public GetUserByIdQueryHandler(UserManager<User> userManager) 
        {
            _userManager = userManager;
        }
        public async Task<OperationResult<User>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            if(request.UserId == Guid.Empty)
            {
                return OperationResult<User>.Fail("User ID is empty", "Application");
            }

            var foundUser = await _userManager.FindByIdAsync(request.UserId.ToString());
             
            if(foundUser == null)
            {
                return OperationResult<User>.Fail("User not found", "Application");
            }

            return OperationResult<User>.Success(foundUser);
        }
    }
}
