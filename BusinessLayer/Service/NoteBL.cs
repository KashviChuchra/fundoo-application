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
        public async Task<ResponseModel<List<NoteModelResponse>>> ViewNotesAsync(int userId)
        {
            var notes = await _noteRL.ViewNotesAsync(userId);
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
                Message = "Notes retrived successfully",
                Data = response
            };
        }
        public async Task<ResponseModel<NoteModelResponse>> ViewNotesByIdAsync(NoteIdRequest note, int userId)
        {
            var note_ = await _noteRL.ViewNotesByIdAsync(note, userId);
            if (note_ == null)
            {
                return new ResponseModel<NoteModelResponse>
                {
                    Success = false,
                    Message = "User or Note doesn't present",
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
                Message = "Notes retrived by Note Id successfully",
                Data = response
            };
        }
        public async Task<ResponseModel<NoteModelResponse>> EditNotesByIdAsync(EditNoteRequest note, int userId)
        {
            var note_ = await _noteRL.EditNotesByIdAsync(note, userId);
            if (note_ == null)
            {
                return new ResponseModel<NoteModelResponse>
                {
                    Success = false,
                    Message = "Note Or User doesn't exist",
                    Data = null
                };
            }
            var response = new NoteModelResponse()
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
                Success=true,
                Message="Note Updated Successfully!",
                Data=response
            };
        }
        public async Task<ResponseModel<NoteModelResponse>> DeleteNotesByIdAsync(NoteIdRequest note, int userId)
        {
            var note_ = await _noteRL.DeleteNotesByIdAsync(note, userId);
            if (note_==null)
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
                Message = "Note Deleted Successfully!",
                Data=response
            };
        }
        public async Task<ResponseModel<NoteModelResponse>> ArchiveNoteAsync(NoteIdRequest noteId, int userId)
        {
            var note_ = await _noteRL.ArchiveNoteAsync(noteId, userId);
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
                Message = "Note Archieved Successfully!",
                Data = response
            };
        }
       
    }
}
