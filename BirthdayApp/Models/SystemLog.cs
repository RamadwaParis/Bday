using System;

namespace BirthdayApp.Models
{
    public class SystemLog
    {
        public int Id { get; set; }
        public string UserEmail { get; set; } = string.Empty;
        public string ActionDetail { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}