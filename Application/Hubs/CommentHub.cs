using Application.Dtos.Comment;
using AutoMapper;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Application.Hubs
{
    public class CommentHub : Hub
    {
        private readonly ILogger<CommentHub> _logger;
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        public CommentHub(IMapper mapper, ILogger<CommentHub> logger, UserManager<User> userManager)
        {
            _mapper = mapper;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task CreateComment(CreateCommentDto commentdto,string UserId)
        {
            var foundUser = await _userManager.FindByIdAsync(UserId);
            if (foundUser == null)
            {
                _logger.LogError("User not found");
                await Clients.Caller.SendAsync("CreateCommentResult", "User not found");
                return;
            }

            var newComment = _mapper.Map<Comment>(commentdto);

            await Clients.All.SendAsync("CommentCreated", newComment);
        }
    }
}
