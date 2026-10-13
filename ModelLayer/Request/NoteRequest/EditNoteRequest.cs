using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Request.NoteRequest
{
    public class EditNoteRequest
    {
        [Required]
        public int NoteId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        [MaxLength(30)]
        public string Colour { get; set; } = string.Empty;

    }
}
