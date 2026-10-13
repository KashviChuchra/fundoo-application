using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Entity
{
    public class NoteEntity
    {
        [Key]
        public int NoteId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        [MaxLength(30)]
        public string Colour { get; set; } = string.Empty;
        public bool IsArchived { get; set; } = false;
        public bool IsDeleted { get; set; } = false;

        [Required]
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public UserEntity User { get; set; } = null!;
        // Tells the compiler that EF Core will populate this navigation property when appropriate. It does not create a user automatically.
    }
}
