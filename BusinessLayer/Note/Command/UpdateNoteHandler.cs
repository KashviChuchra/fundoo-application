using BusinessLayer.Note.Command;
using BusinessLayer.Note.Command.Handler_Interface;
using ModelLayer.Response.NoteResponse;
using ModelLayer.Response.UserResponse;
using RepositoryLayer.Interface;

namespace BusinessLayer.Note.Handler
{
    public class UpdateNoteHandler: IUpdateNoteHandler
    {
        private readonly INoteRL _noteRL;

        public UpdateNoteHandler(INoteRL noteRL)
        {
            _noteRL = noteRL;
        }

        public async Task<ResponseModel<NoteModelResponse>> HandleAsync(UpdateNoteCommand command)
        {
            var note_ = await _noteRL.EditNotesByIdAsync(command.Note,command.UserId);

            if (note_ == null)
            {
                return new ResponseModel<NoteModelResponse>
                {
                    Success = false,
                    Message = "Note Or User doesn't exist",
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
                Message = "Note Updated Successfully!",
                Data = response
            };
        }
    }
}
