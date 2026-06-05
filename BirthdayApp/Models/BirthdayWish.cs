using Microsoft.AspNetCore.Mvc;

using System.ComponentModel.DataAnnotations;

namespace BirthdayApp.Models
{
    public class BirthdayWish
    {
        public int Id { get; set; }
        public int BirthdayId { get; set; }

        [Required]
        public string? SenderName { get; set; }

        [Required]
        public string? Message { get; set; }

        public DateTime SentAt { get; set; }
    }
}