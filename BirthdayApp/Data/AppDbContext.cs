using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BirthdayApp.Models;

namespace BirthdayApp.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Birthday> Birthdays { get; set; }
        public DbSet<SystemLog> SystemLogs { get; set; }

        // Add these two lines:
        public DbSet<WishTemplate> WishTemplates { get; set; }
        public DbSet<BirthdayWish> BirthdayWishes { get; set; }
    }
}