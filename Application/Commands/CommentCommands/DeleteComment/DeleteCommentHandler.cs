using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.CommentCommands.DeleteComment
{
    public class DeleteCommentHandler : IRequestHandler<DeleteCommentCommand, OperationResult<Guid>>
    {
        private readonly IGenericRepository<Comment> _commentRepository;
        private readonly ILogger<DeleteCommentHandler> _logger;
        public DeleteCommentHandler(IGenericRepository<Comment> commentRepository, ILogger<DeleteCommentHandler> logger)
        {
            _commentRepository = commentRepository;
            _logger = logger;
        }
        public async Task<OperationResult<Guid>> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.CommentId == null || request.CommentId == Guid.Empty)
                {
                    _logger.LogError("Comment not found");
                    return OperationResult<Guid>.Fail("Comment not found", "Applicaton");
                }

                var foundComment = await _commentRepository.GetByIdAsync(request.CommentId);

                if (foundComment == null)
                {
                    _logger.LogError("Could not find given comment.");
                    return OperationResult<Guid>.Fail("Could not find given comment.", "Application");
                }
                var result = await _commentRepository.DeleteAsync(foundComment);

                if (result != null)
                {
                    _logger.LogError("Could not delete comment.");
                    return OperationResult<Guid>.Fail("Could not delete comment.", "Application");
                }
                _logger.LogInformation("Successfully deleted comment.");

                return OperationResult<Guid>.Success(request.CommentId);
            }
            catch
            {
                _logger.LogError("Unexpected error");
                return OperationResult<Guid>.Fail("Unexpected error", "Applicaton");
            }
        }
    }
}
