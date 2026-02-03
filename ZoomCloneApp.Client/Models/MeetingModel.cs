using System.ComponentModel.DataAnnotations;

namespace ZoomCloneApp.Client.Models
{
    public class MeetingModel
    {
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public DateTime? StartDate { get; set; }
        [Required]
        public DateTime? StartTime { get; set; }
        [Required]
        public DateTime? EndDate { get; set; }
        [Required]
        public DateTime? EndTime { get; set; }
    }
}
