
using ModelLayer.Request.NoteRequest;

namespace BusinessLayer.Note.Command
{
    public class CreateNoteCommand
    {
        public NoteCreationRequest Note { get;}
        public int UserId { get;}
        public CreateNoteCommand(NoteCreationRequest note, int userId)
        {
            Note = note;
            UserId = userId;
        }
    }
}
