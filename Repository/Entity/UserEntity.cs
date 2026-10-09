using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public ICollection<PasswordResetTokenEntity> PasswordResetTokenEntities { get; set; }= new List<PasswordResetTokenEntity>();
        //One user can have many password-reset tokens.
    }
}

// pk - key
// not null - required

// database - table 