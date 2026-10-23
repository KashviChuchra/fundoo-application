namespace BusinessLayer.Note.Query
{
    public class GetAllNotesQuery
    {
        public int UserId { get; }

        public GetAllNotesQuery(int userId)
        {
            UserId = userId;
        }
    }
}