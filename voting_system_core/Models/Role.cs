using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace voting_system_core.Models
{
    [Table("role")]
    public class Role
    {
        [Key]
        public sbyte Key { get; set; }
        public string Value { get; set; }
        public string Authority { get; set; }
    }
}
