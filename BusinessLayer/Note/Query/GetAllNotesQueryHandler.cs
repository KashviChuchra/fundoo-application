using ModelLayer.Response.NoteResponse;
using ModelLayer.Response.UserResponse;
using RepositoryLayer.Interface;

namespace BusinessLayer.Note.Query
{
    public class GetAllNotesQueryHandler: IGetAllNotesQueryHandler
    {
        private readonly INoteRL _noteRL;

        public GetAllNotesQueryHandler(INoteRL noteRL)
        {
            _noteRL = noteRL;
        }

        public async Task<ResponseModel<List<NoteModelResponse>>> HandleAsync(GetAllNotesQuery query)
        {
            var notes = await _noteRL.ViewNotesAsync(query.UserId);

            var response = notes.Select(x => new NoteModelResponse
            {
                NoteId = x.NoteId,
                Title = x.Title,
                Description = x.Description,
                Colour = x.Colour,
                IsArchived = x.IsArchived,
                IsDeleted = x.IsDeleted
            }).ToList();

            return new ResponseModel<List<NoteModelResponse>>
            {
                Success = true,
                Message = "Notes retrieved successfully",
                Data = response
            };
        }
    }
}