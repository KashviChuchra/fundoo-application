using ModelLayer.Request.NoteRequest;
using ModelLayer.Response;
using ModelLayer.Response.NoteResponse;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Interface
{
    public interface INoteRL
    {
        Task<NoteEntity?> AddNoteAsync(NoteCreationRequest note, int userId);
        Task<List<NoteEntity>> ViewNotesAsync(int userId);
        Task<NoteEntity?> ViewNotesByIdAsync(NoteIdRequest note, int userId);
        Task<NoteEntity?> EditNotesByIdAsync(EditNoteRequest note, int userId);
        Task<NoteEntity?> DeleteNotesByIdAsync(NoteIdRequest note, int userId);
        Task<NoteEntity?> ArchiveNoteAsync(NoteIdRequest noteId, int userId);
    }
}
