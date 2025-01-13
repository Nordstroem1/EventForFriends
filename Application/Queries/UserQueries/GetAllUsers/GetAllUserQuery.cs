using Domain.Models;
using MediatR;

namespace Application.Queries.UserQueries.GetAllUsers
{
    public class GetAllUserQuery : IRequest<OperationResult<List<User>>>
    {
        public List<User> Users { get; set; }
        public GetAllUserQuery() 
        {
            Users = new List<User>();
        }
    }
}
