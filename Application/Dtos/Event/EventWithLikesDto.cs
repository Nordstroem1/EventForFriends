using Application.Dtos.User;

namespace Application.Dtos.Event
{
    public class EventWithLikesDto
    {
        public string EventId { get; set; }
        public string EventName { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsclosedEvent { get; set; }
        public string CreatedBy { get; set; }
        public string Location { get; set; }
        public List<UserLikeDto> LikeList { get; set; }
    }
}
