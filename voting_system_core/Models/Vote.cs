﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace voting_system_core.Models
{
    [Table("vote")]
    public class Vote
    {
        [Key]
        public Ulid VoteId { get; set; } 
        public string Email { get; set; }
        public bool IsVerified { get; set; }

        [ForeignKey("PollId")]
        public Ulid PollId { get; set; }
        [NotMapped]
        public Poll Poll { get; set; }

        [ForeignKey("OptionId")]
        public Ulid OptionId { get; set; }
        [NotMapped]
        public Option Option { get; set; }
    }
}
