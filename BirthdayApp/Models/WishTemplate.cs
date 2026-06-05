using Microsoft.AspNetCore.Mvc;

namespace BirthdayApp.Models
{
    public class WishTemplate
    {
        public int Id { get; set; }

        // The '?' allows the database to accept nulls and stops the compiler warning
        public string? Content { get; set; }
    }
}
