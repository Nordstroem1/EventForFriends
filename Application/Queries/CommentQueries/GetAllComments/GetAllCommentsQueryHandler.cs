using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Queries.CommentQueries.GetAllComments
{
    public class GetAllCommentsQueryHandler : IRequestHandler<GetAllCommentsQuery, OperationResult<List<Comment>>>
    {
        private readonly IGenericRepository<Comment> _commentRepository;
        private readonly IGenericRepository<Event> _eventRepository;
        private readonly ILogger<GetAllCommentsQueryHandler> _logger;
        public GetAllCommentsQueryHandler(IGenericRepository<Comment> commentRepository, IGenericRepository<Event> eventRepository, ILogger<GetAllCommentsQueryHandler> logger)
        {
            _commentRepository = commentRepository;
            _eventRepository = eventRepository;
            _logger = logger;
        }
        public async Task<OperationResult<List<Comment>>> Handle(GetAllCommentsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.EventId == Guid.Empty)
                {
                    _logger.LogError("EventId can't be null");
                    return OperationResult<List<Comment>>.Fail("EventId can't be null", "Application");
                }

                var foundEvent = await _eventRepository.GetByIdAsync(request.EventId);

                if (foundEvent == null)
                {
                    _logger.LogError("Could not find event");
                    return OperationResult<List<Comment>>.Fail("Could not find event", "Application");
                }

                var comments = await _commentRepository.Find(x => x.EventId == request.EventId);

                if (comments == null)
                {
                    _logger.LogError("Could not find comments");
                    return OperationResult<List<Comment>>.Fail("Could not find comments", "Application");
                }
                if (comments.Count() <= 0)
                {
                    _logger.LogInformation("No Comments made yet");
                    return OperationResult<List<Comment>>.Success(new List<Comment>());
                }
                _logger.LogInformation("Comments retrieved successfully");

                return OperationResult<List<Comment>>.Success(comments.ToList());
            }
            catch
            {
                _logger.LogError("Unexpected error");
                return OperationResult<List<Comment>>.Fail("Unexpected error", "Application");
            }
        }
    }
}