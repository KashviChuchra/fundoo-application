using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Entity
{
    public class PasswordResetTokenEntity
    {
        [Key]
        public int ResetTokenId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public string TokenHash { get; set; } = string.Empty;

        [Required]
        public DateTime ExpiryTime { get; set; }

        public bool IsUsed { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(UserId))]
        public UserEntity User { get; set; } = null!;

    }
}
