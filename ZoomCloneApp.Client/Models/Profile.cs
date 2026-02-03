using System.ComponentModel.DataAnnotations;

namespace ZoomCloneApp.Client.Models
{
    public class Profile
    {
        [Required]
        public string Name { get; set; } = String.Empty;
    }
}
