using BusinessLayer.Note.Handler;
using ModelLayer.Request.NoteRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace BusinessLayer.Note.Command
{
    public class DeleteNoteCommand
    {
        public NoteIdRequest Note { get; }
        public int UserId { get; }
        public DeleteNoteCommand(NoteIdRequest note, int userId)
        {
            Note = note;
            UserId = userId;
        }
        
    }
}
