using Application.Dtos.Event;
using Application.Dtos.User;
using AutoMapper;
using Domain.Interfaces;
using Domain.Models;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics.Tracing;

namespace Application.Queries.EventQueries.GetEventById
{
    public class GetEventByIdQueryHandler : IRequestHandler<GetEventbyIdQuery, OperationResult<EventWithLikesDto>>
    {
        private readonly IGenericRepository<Event> _eventRepository;
        private readonly ILogger<GetEventByIdQueryHandler> _logger;
        private readonly IMapper _mapper;
        public GetEventByIdQueryHandler(IMapper mapper, IGenericRepository<Event> eventRepository, ILogger<GetEventByIdQueryHandler> logger)
        {
            _mapper = mapper;
            _eventRepository = eventRepository;
            _logger = logger;
        }
        public async Task<OperationResult<EventWithLikesDto>> Handle(GetEventbyIdQuery request, CancellationToken cancellationToken)
        {
            if (request.EventId == string.Empty || request == null)
            {
                _logger.LogError("No id was given");
                return OperationResult<EventWithLikesDto>.Fail("No id was given", "Application");
            }

            var result = await _eventRepository.FindWithIncludes(e => e.EventId == request.EventId
                                                                ,e => e.LikeList);

            if (result == null || !result.Any())
            {
                _logger.LogError($"No event with id {request.EventId}");
                return OperationResult<EventWithLikesDto>.Fail("No event with that id", "Application");
            }

            var eventEntity = result.FirstOrDefault();
            var eventDto = _mapper.Map<EventWithLikesDto>(eventEntity);

            return OperationResult<EventWithLikesDto>.Success(eventDto);
        }
    }
}
