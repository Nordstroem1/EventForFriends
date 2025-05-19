using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace Application.Dtos.Event
{
    public class CreateEventDto
    {
        [Required(ErrorMessage = "The eventname is to long or short")]
        [MaxLength(50)]
        [MinLength(2)]
        public string EventName { get; set; }
        [Required]
        [MaxLength(700)]
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        public IFormFile? Image { get; set; }
        public bool IsclosedEvent { get; set; }
        [MaxLength(50)]
        public string Location { get; set; }
    }
}
