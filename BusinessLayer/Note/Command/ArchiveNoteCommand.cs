using BusinessLayer.Note.Command.Handler_Interface;
using ModelLayer.Request.NoteRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Note.Command
{
    public class ArchiveNoteCommand
    {
        public NoteIdRequest Note { get; }
        public int UserId { get; }
        public ArchiveNoteCommand(NoteIdRequest note, int userId)
        {
            Note = note;
            UserId = userId;
        }
    }
}
