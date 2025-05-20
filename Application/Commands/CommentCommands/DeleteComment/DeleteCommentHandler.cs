using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.Commands.CommentCommands.DeleteComment
{
    public class DeleteCommentHandler : IRequestHandler<DeleteCommentCommand, OperationResult<string>>
    {
        private readonly IGenericRepository<Comment> _commentRepository;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<DeleteCommentHandler> _logger;
        public DeleteCommentHandler(UserManager<User> userManager, IGenericRepository<Comment> commentRepository, ILogger<DeleteCommentHandler> logger)
        {
            _userManager = userManager;
            _commentRepository = commentRepository;
            _logger = logger;
        }
        public async Task<OperationResult<string>> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.CommentId == null || request.CommentId == string.Empty)
                {
                    _logger.LogError("Comment not found");
                    return OperationResult<string>.Fail("Comment not found", "Applicaton");
                }

                var foundUser = await _userManager.FindByIdAsync(request.UserId);
                var foundComment = await _commentRepository.GetByIdAsync(request.CommentId);

                if (foundComment == null)
                {
                    _logger.LogError("Could not find given comment.");
                    return OperationResult<string>.Fail("Could not find given comment.", "Application");
                }
                if (foundUser == null)
                {
                    _logger.LogError("Could not find given user.");
                    return OperationResult<string>.Fail("Could not find given user.", "Application");
                }

                if(foundUser.Id != foundComment.UserId && 
                    foundUser.Role.ToLower() != "admin" &&
                    foundUser.Role.ToLower() != "superadmin")
                {
                    _logger.LogError("User does not have permission to delete this comment.");
                    return OperationResult<string>.Fail("User does not have permission to delete this comment.", "Application");
                }

                var result = await _commentRepository.DeleteAsync(foundComment);

                _logger.LogInformation("Successfully deleted comment.");

                return OperationResult<string>.Success(request.CommentId);
            }
            catch
            {
                _logger.LogError("Unexpected error");
                return OperationResult<string>.Fail("Unexpected error", "Applicaton");
            }
        }
    }
}
