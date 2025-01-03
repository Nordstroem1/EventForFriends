using Application.Dtos.Comment;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Commands.CommentCommands.UpdateComment
{
    public class UpdateCommentHandler : IRequestHandler<UpdateCommentCommand, OperationResult<UpdateCommentDto>>
    {
        private readonly ILogger<UpdateCommentHandler> _logger;
        private readonly IGenericRepository<Comment> _commentRepsitory;

        public UpdateCommentHandler(ILogger<UpdateCommentHandler> logger, IGenericRepository<Comment> commentRepsitory)
        {
            _logger = logger;
            _commentRepsitory = commentRepsitory;
        }
        public async Task<OperationResult<UpdateCommentDto>> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.CommentId == null || request.CommentId == Guid.Empty)
                {
                    _logger.LogError("Comment not found");
                    return OperationResult<UpdateCommentDto>.Fail("Comment not found", "Application");
                }

                var foundComment = await _commentRepsitory.GetByIdAsync(request.CommentId);

                if (foundComment == null)
                {
                    _logger.LogError("Comment not found");
                    return OperationResult<UpdateCommentDto>.Fail("Comment not found", "Application");
                }

                foundComment.CommentContent = request.UpdateCommentDto.CommentContent;

                var result = await _commentRepsitory.UpdateAsync(foundComment);

                if (result == null)
                {
                    _logger.LogError("Could not update comment");
                    return OperationResult<UpdateCommentDto>.Fail("Could not update comment", "Application");
                }

                _logger.LogInformation("Comment updated successfully");

                return OperationResult<UpdateCommentDto>.Success(request.UpdateCommentDto);
            }
            catch
            {
                _logger.LogError("Unexpected error");
                return OperationResult<UpdateCommentDto>.Fail("Unexpected error", "Application");
            }
        }
    }
}
