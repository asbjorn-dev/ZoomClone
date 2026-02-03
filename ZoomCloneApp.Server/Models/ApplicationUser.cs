using Microsoft.AspNetCore.Identity;

namespace ZoomCloneApp.Server.Models
{
    /// Server-side user model with Identity integration
    public class ApplicationUser : IdentityUser 
    {

        public string Name { get; set; } = string.Empty;
    }
}