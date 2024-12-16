using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Event
    {
        [Required]
        public Guid EventId { get; set; }
        [Required(ErrorMessage = "The eventname is to long or short")]
        [MaxLength(50)]
        [MinLength(2)]
        public string EventName { get; set; }
        [Required(ErrorMessage = "Discription to long")]
        [MaxLength(700)]
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "Event must have an end date")]
        public DateTime EndDate { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsclosedEvent { get; set; }
        public Event(){}
        public Event(Guid eventId, string eventName, string description, DateTime createdAt, DateTime startDate, DateTime endDate, string imageUrl, bool isclosedEvent)
        {
            EventId = eventId;
            EventName = eventName;
            Description = description;
            CreatedAt = createdAt;
            StartDate = startDate;
            EndDate = endDate;
            ImageUrl = imageUrl;
            IsclosedEvent = isclosedEvent;
        }
    }
}
