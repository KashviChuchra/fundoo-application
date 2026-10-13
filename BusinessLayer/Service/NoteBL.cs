using BusinessLayer.Interface;
using ModelLayer.Request.NoteRequest;
using ModelLayer.Response;
using ModelLayer.Response.NoteResponse;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Service
{
    public class NoteBL: INoteBL
    {
        private readonly INoteRL _noteRL;
        public NoteBL(INoteRL noteRL)
        {
            _noteRL = noteRL;
        }

        public async Task<ResponseModel<NoteModelResponse>> AddNoteAsync(NoteCreationRequest note, int userId)
        {
            var new_note = await _noteRL.AddNoteAsync(note, userId);

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
        public Task<ResponseModel<List<NoteModelResponse>>> ViewNotesAsync(int userId)
        {
            throw new NotImplementedException();
        }
        public Task<ResponseModel<NoteModelResponse>> ViewNotesByIdAsync(NoteIdRequest note, int userId)
        {
            throw new NotImplementedException();
        }
        public Task<ResponseModel<NoteModelResponse>> EditNotesByIdAsync(EditNoteRequest note, int userId)
        {
            throw new NotImplementedException();
        }
        public Task<ResponseModel<NoteModelResponse>> DeleteNotesByIdAsync(NoteIdRequest note, int userId)
        {
            throw new NotImplementedException();
        }
        public Task<ResponseModel<NoteModelResponse>> ArchiveNoteAsync(NoteIdRequest noteId, int userId)
        {
            throw new NotImplementedException();
        }
        public Task<ResponseModel<NoteModelResponse>> TrashUnTrashNoteAsync(NoteIdRequest noteId, int userId)
        {
            throw new NotImplementedException();
        }
    }
}
