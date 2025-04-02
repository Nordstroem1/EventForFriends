using AutoMapper;
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
        private readonly IGenericRepository<Event> _eventRepository;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<CreateCommentHandler> _logger;
        private readonly IMapper _mapper;
        public CreateCommentHandler(IGenericRepository<Comment> commentRepository,IGenericRepository<Event> eventrepository, UserManager<User> userManager, ILogger<CreateCommentHandler> logger, IMapper mapper)
        {
            _commentRepository = commentRepository;
            _eventRepository = eventrepository;
            _userManager = userManager;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<OperationResult<Comment>> Handle(CreateComment request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.UserID == null || request.UserID == string.Empty)
                {
                    _logger.LogError("User not found");
                    return OperationResult<Comment>.Fail("User not found", "Applicaton");
                }

                var foundUser = await _userManager.FindByIdAsync(request.UserID);

                if (foundUser == null)
                {
                    _logger.LogError("User not found");
                    return OperationResult<Comment>.Fail("User not found", "Applicaton");
                }


                var newComment = new Comment
                {
                    CommentId = Guid.NewGuid().ToString(),
                    CommentContent = request.CommentDto.CommentContent,
                    TimeSent = DateTime.Now,
                    UserId = foundUser.Id,
                    EventId = request.CommentDto.EventId,
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
