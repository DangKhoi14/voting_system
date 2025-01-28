using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace voting_system_core.Models
{
    [Table("option")]
    public class Option
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Ulid OptionId { get; set; }
        public string OptionText { get; set; }

        [ForeignKey("PollId")]
        public Ulid PollId { get; set; }
        [NotMapped]
        public Poll Poll { get; set; }

        // Navigation Property
        [NotMapped]
        public List<Vote> Votes { get; set; }
    }
}
