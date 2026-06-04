using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BirthdayApp.Models;

namespace BirthdayApp.Data
{
    // Adding ': IdentityDbContext<ApplicationUser>' satisfies the generic constraint 
    // and fixes the CS0311 error in Program.cs
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Birthday> Birthdays { get; set; }
        public DbSet<SystemLog> SystemLogs { get; set; }
    }
}