using BusinessLayer.Note.Command;
using BusinessLayer.Note.Command.Handler_Interface;
using ModelLayer.Response.NoteResponse;
using ModelLayer.Response.UserResponse;
using RepositoryLayer.Interface;

namespace BusinessLayer.Note.Handler
{
    public class CreateNoteHandler: ICreateNoteHandler
    {
        private readonly INoteRL _noteRL;

        public CreateNoteHandler(INoteRL noteRL)
        {
            _noteRL = noteRL;
        }

        public async Task<ResponseModel<NoteModelResponse>> HandleAsync(CreateNoteCommand command)
        {
            var new_note = await _noteRL.AddNoteAsync(command.Note,command.UserId);

            if (new_note == null)
            {
                return new ResponseModel<NoteModelResponse>
                {
                    Success = false,
                    Message = "User doesn't exist",
                    Data = null
                };
            }

            var response = new NoteModelResponse
            {
                NoteId = new_note.NoteId,
                Title = new_note.Title,
                Description = new_note.Description,
                Colour = new_note.Colour,
                IsArchived = new_note.IsArchived,
                IsDeleted = new_note.IsDeleted
            };

            return new ResponseModel<NoteModelResponse>
            {
                Success = true,
                Message = "Note created successfully",
                Data = response
            };
        }
    }
}