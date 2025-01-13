using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.CommentQueries.GetCommentById
{
    public class GetCommentByIdQueryHandler : IRequestHandler<GetCommentByIdQuery, OperationResult<Comment>>
    {
        private readonly IGenericRepository<Comment> _repository;
        private readonly ILogger<GetCommentByIdQueryHandler> _logger;
        public GetCommentByIdQueryHandler(IGenericRepository<Comment> repository, ILogger<GetCommentByIdQueryHandler> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        public async Task<OperationResult<Comment>> Handle(GetCommentByIdQuery request, CancellationToken cancellationToken)
        {
            var foundComment = await _repository.GetByIdAsync(request.CommentId);

            if(foundComment == null)
            {
                _logger.LogError("The given comment does not exist");
                return OperationResult<Comment>.Fail("The given comment does not exist", "Application");
            }
            _logger.LogInformation("Successfully returned the given comment.");
            return OperationResult<Comment>.Success(foundComment);
        }
    }
}
