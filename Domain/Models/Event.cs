using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class Event
    {
        [Required]
        public Guid EventId { get; set; }
        [Required]
        [MaxLength(50)]
        [MinLength(2)]
        public string EventName { get; set; }
        [Required]
        [MaxLength(700)]
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsclosedEvent { get; set; }
        public string CreatedBy { get; set; }
        public string Location { get; set; }
        public List<User> LikeList { get; set; } = new List<User>();

        public Event(){}
        public Event(Guid eventId, string eventName, string description, DateTime createdAt, DateTime startDate, DateTime endDate, string imageUrl, bool isclosedEvent, string createdBy,  string location)
        {
            EventId = eventId;
            EventName = eventName;
            Description = description;
            CreatedAt = createdAt;
            StartDate = startDate;
            EndDate = endDate;
            ImageUrl = imageUrl;
            IsclosedEvent = isclosedEvent;
            CreatedBy = createdBy;
            Location = location;
        }
    }
}
