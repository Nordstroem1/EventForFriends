using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Event
{
    public class UpdateEventDto
    {
        [Required]
        [MaxLength(50)]
        [MinLength(2)]
        public string EventName { get; set; }
        [Required]
        [MaxLength(700)]
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsclosedEvent { get; set; }
        public string CreatedBy { get; set; }
        public string Location { get; set; }

        public UpdateEventDto() { }
        public UpdateEventDto(string eventName, string description, DateTime startDate, DateTime endDate, string imageUrl, bool isclosedEvent, string createdBy, string location)
        {
            EventName = eventName;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
            ImageUrl = imageUrl;
            IsclosedEvent = isclosedEvent;
            CreatedBy = createdBy;
            Location = location;
        }
    }
}

