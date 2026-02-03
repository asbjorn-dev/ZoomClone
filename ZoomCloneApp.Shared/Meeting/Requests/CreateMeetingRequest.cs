using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZoomCloneApp.Shared.Meeting.Requests
{
    public class CreateMeetingRequest
    {
        [Required]
        public string HostId { get; set; } = string.Empty;
        [Required]
        public string Title { get; set; } = string.Empty;
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public string StartTimeOnly { get; set; } = string.Empty;
        [Required]
        public string EndTimeOnly { get; set; } = string.Empty;
        [Required]
        public string StartDateOnly { get; set; } = string.Empty;
        [Required]
        public string EndDateOnly { get; set; } = string.Empty;
    }
}
