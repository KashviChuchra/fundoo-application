using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace RepositoryLayer.Entity
{
    public class UserEntity
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        [Required]
       // [Index(IsUnique = true)] // to make sure that email is unique
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        // Adding forgot-reset password
        public string? ResetToken { get; set; }

        public DateTime? ResetTokenExpiry { get; set; }

        public bool IsResetTokenUsed { get; set; }

    }
}

// pk - key
// not null - required

// database - table 