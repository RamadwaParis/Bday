using System;

namespace BirthdayApp.Models
{
    public class Birthday
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }

        // Tracks which user added this record
        public string CreatedBy { get; set; } = string.Empty;

        // Added for Soft Delete pattern
        public bool IsDeleted { get; set; } = false;
    }
}