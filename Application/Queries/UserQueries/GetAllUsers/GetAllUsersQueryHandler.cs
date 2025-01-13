using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.UserQueries.GetAllUsers
{
    public class GetAllUsersQueryHandler : IRequestHandler<GetAllUserQuery, OperationResult<List<User>>>
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly ILogger<GetAllUsersQueryHandler> _logger;

        public GetAllUsersQueryHandler(IGenericRepository<User> userRepository, ILogger<GetAllUsersQueryHandler> logger)
        {
            _logger = logger;
            _userRepository = userRepository;
        }
        public async Task<OperationResult<List<User>>> Handle(GetAllUserQuery request, CancellationToken cancellationToken)
        {
            var UserList = await _userRepository.GetAllAsync();

            if (UserList == null || UserList.Count() <= 0)
            {
                _logger.LogError("No users in List");
                return OperationResult<List<User>>.Fail("No users in list", "Application");
            }
            
            return OperationResult<List<User>>.Success(UserList.ToList());
        }
    }
}
