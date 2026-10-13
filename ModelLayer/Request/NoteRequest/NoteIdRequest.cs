using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Request.NoteRequest
{
    public class NoteIdRequest
    {
        [Required]
        public int NoteId { get; set; }
    }

}
