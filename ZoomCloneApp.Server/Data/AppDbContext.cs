using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ZoomCloneApp.Server.Models;

namespace ZoomCloneApp.Server.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSets 
        public DbSet<Meeting> Meetings { get; set; }
    }
}