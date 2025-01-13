using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.Commands.CommentCommands.CreateComment
{
    public class CreateCommentHandler : IRequestHandler<CreateComment, OperationResult<Comment>>
    {
        private readonly IGenericRepository<Comment> _commentRepository;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ILogger<CreateCommentHandler> _logger;
        public CreateCommentHandler(IGenericRepository<Comment> commentRepository, UserManager<IdentityUser> userManager, ILogger<CreateCommentHandler> logger)
        {
            _commentRepository = commentRepository;
            _userManager = userManager;
            _logger = logger;
        }
        public async Task<OperationResult<Comment>> Handle(CreateComment request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.UserID == null || request.UserID == Guid.Empty)
                {
                    _logger.LogError("User not found");
                    return OperationResult<Comment>.Fail("User not found", "Applicaton");
                }

                var foundUser = await _userManager.FindByIdAsync(request.UserID.ToString());

                if (foundUser == null)
                {
                    _logger.LogError("User not found");
                    return OperationResult<Comment>.Fail("User not found", "Applicaton");
                }

                var newComment = new Comment
                {
                    CommentId = Guid.NewGuid(),
                    CommentContent = request.CommentDto.CommentContent,
                    TimeSent = request.CommentDto.TimeSent,
                    EventId = Guid.Parse(foundUser.Id),
                    Likes = 0
                };

                var result = await _commentRepository.AddAsync(newComment);
                _logger.LogInformation("Comment created successfully");

                return OperationResult<Comment>.Success(result);
            }
            catch
            {
                _logger.LogError("Unexpected error");
                return OperationResult<Comment>.Fail("Unexpected error", "Applicaton");
            }
        }
    }
}
