﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace voting_system_core.Models
{
    [Table("poll")]
    public class Poll
    {
        [Key]
        public Ulid PollId { get; set; }

        [ForeignKey("UserId")]
        public Guid UserId { get; set; }
        
        [NotMapped]
        public Account Account { get; set; }
        
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        [NotMapped]
        public bool IsActive => DateTime.Now >= StartTime && DateTime.Now <= EndTime;

        // Navigation Properties
        [NotMapped]
        public List<Option> Options { get; set; }
        
        [NotMapped]
        public List<Vote> Votes { get; set; }
    }
}
