namespace Application.Dtos.Event
{
    public class LikeEventDto
    {
        public Guid EventId { get; set; }
        public Guid UserId { get; set; }
    }
}
