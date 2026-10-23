using ModelLayer.Response.NoteResponse;
using ModelLayer.Response.UserResponse;
using RepositoryLayer.Interface;

namespace BusinessLayer.Note.Query
{
    public class GetNotesByIdQueryHandler: IGetNotesByIdQueryHandler
    {
        private readonly INoteRL _noteRL;

        public GetNotesByIdQueryHandler(INoteRL noteRL)
        {
            _noteRL = noteRL;
        }

        public async Task<ResponseModel<NoteModelResponse>> HandleAsync(GetNotesByIdQuery query)
        {
            var note_ = await _noteRL.ViewNotesByIdAsync(
                query.Note,
                query.UserId);

            if (note_ == null)
            {
                return new ResponseModel<NoteModelResponse>
                {
                    Success = false,
                    Message = "User or Note doesn't exist",
                    Data = null
                };
            }

            var response = new NoteModelResponse
            {
                NoteId = note_.NoteId,
                Title = note_.Title,
                Description = note_.Description,
                Colour = note_.Colour,
                IsArchived = note_.IsArchived,
                IsDeleted = note_.IsDeleted
            };

            return new ResponseModel<NoteModelResponse>
            {
                Success = true,
                Message = "Notes retrieved by Note Id successfully",
                Data = response
            };
        }
    }
}