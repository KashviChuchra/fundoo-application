using ModelLayer.Request.NoteRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Note.Command
{
    public class UpdateNoteCommand
    {
        public EditNoteRequest Note { get; }
        public int UserId { get; }
        public UpdateNoteCommand(EditNoteRequest note, int userId)
        {
            Note = note;
            UserId = userId;
        }
    }
}
