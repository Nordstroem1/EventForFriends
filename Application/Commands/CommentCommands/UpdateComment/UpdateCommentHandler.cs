using Application.Dtos.Comment;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.Commands.CommentCommands.UpdateComment
{
    public class UpdateCommentHandler : IRequestHandler<UpdateCommentCommand, OperationResult<Comment>>
    {
        private readonly ILogger<UpdateCommentHandler> _logger;
        private readonly IGenericRepository<Comment> _commentRepsitory;
        private readonly UserManager<User> _userManager;

        public UpdateCommentHandler(UserManager<User> userManager, ILogger<UpdateCommentHandler> logger, IGenericRepository<Comment> commentRepsitory)
        {
            _userManager = userManager;
            _logger = logger;
            _commentRepsitory = commentRepsitory;
        }
        public async Task<OperationResult<Comment>> Handle(UpdateCommentCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.CommentId == null || request.CommentId == string.Empty)
                {
                    _logger.LogError("Comment not found");
                    return OperationResult<Comment>.Fail("Comment not found", "Application");
                }

                var foundComment = await _commentRepsitory.GetByIdAsync(request.CommentId);
                var foundUser = await _userManager.FindByIdAsync(request.UserId);

                if (foundComment == null)
                {
                    _logger.LogError("Comment not found");
                    return OperationResult<Comment>.Fail("Comment not found", "Application");
                }
                if (foundUser == null)
                {
                    _logger.LogError("User not found");
                    return OperationResult<Comment>.Fail("User not found", "Application");
                }

                if (foundUser.Id != foundComment.UserId && 
                    foundUser.Role.ToLower() != "admin" &&
                    foundUser.Role.ToLower() != "superadmin")
                {
                    _logger.LogError("User does not have permission to update this comment");
                    return OperationResult<Comment>.Fail("User does not have permission to update this comment", "Application");
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
