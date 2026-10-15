using ModelLayer.Request.NoteRequest;
using ModelLayer.Response.NoteResponse;
using ModelLayer.Response.UserResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interface
{
    public interface INoteBL
    {
        Task<ResponseModel<NoteModelResponse>> AddNoteAsync(NoteCreationRequest note, int userId);
        Task<ResponseModel<List<NoteModelResponse>>> ViewNotesAsync(int userId);
        Task<ResponseModel<NoteModelResponse>> ViewNotesByIdAsync(NoteIdRequest note, int userId);
        Task<ResponseModel<NoteModelResponse>> EditNotesByIdAsync(EditNoteRequest note, int userId);
        Task<ResponseModel<NoteModelResponse>> DeleteNotesByIdAsync(NoteIdRequest note, int userId);
        Task<ResponseModel<NoteModelResponse>> ArchiveNoteAsync(NoteIdRequest noteId, int userId);
    }
}
