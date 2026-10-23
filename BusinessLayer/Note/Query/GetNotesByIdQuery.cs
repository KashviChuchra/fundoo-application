using ModelLayer.Request.NoteRequest;

namespace BusinessLayer.Note.Query
{
    public class GetNotesByIdQuery
    {
        public NoteIdRequest Note { get; }
        public int UserId { get; }

        public GetNotesByIdQuery(NoteIdRequest note, int userId)
        {
            Note = note;
            UserId = userId;
        }
    }
}