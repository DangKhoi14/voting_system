﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace voting_system_core.Models
{
    [Table("account")]
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid UserId { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        //public string PasswordHash { get; set; }
        //public string Salt { get; set; }

        public string? Email { get; set; }

        public string FirstEmail { get; set; }

        public DateOnly CreateAt { get; set; }

        public bool IsEmailVerified { get; set; }

        // Additional attributes
        [ForeignKey("Role")]
        public sbyte Role { get; set; }

        public DateTime LastLogin { get; set; }

        //public bool IsActive { get; set; } = true;

        public bool IsDeleted { get; set; }

        public string? Bio { get; set; }

        public string? ProfilePictureUrl { get; set; }

        public string? ResetPasswordToken { get; set; }
    }
}
