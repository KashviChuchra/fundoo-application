using Microsoft.EntityFrameworkCore;
using ModelLayer.Request.NoteRequest;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Service
{
    public class NoteRL : INoteRL
    {
        private readonly FundooContext _fundoocontext;
        public NoteRL(FundooContext fundooContext)
        {
            _fundoocontext = fundooContext;
        }

        public async Task<NoteEntity> AddNoteAsync(NoteCreationRequest note, int userId)
        {
            var user_exists = await _fundoocontext.Users.AnyAsync(x => x.UserId == userId);
            if (!user_exists)
            {
                throw new KeyNotFoundException("User Not Found");
            }
            var new_note = new NoteEntity
            {
                Title = note.Title,
                Colour= note.Colour,
                Description=note.Description,
                UserId=userId
            };

            await _fundoocontext.AddAsync(new_note);
            await _fundoocontext.SaveChangesAsync();
            return new_note;
        }
        public Task<List<NoteEntity>> ViewNotesAsync(int userId)
        {
            throw new NotImplementedException();
        }
        public Task<NoteEntity?> ViewNotesByIdAsync(NoteIdRequest note, int userId)
        {
            throw new NotImplementedException();
        }
        public Task<NoteEntity?> EditNotesByIdAsync(EditNoteRequest note, int userId)
        {
            throw new NotImplementedException();
        }
        public Task<bool> DeleteNotesByIdAsync(NoteIdRequest note, int userId)
        {
            throw new NotImplementedException();
        }
        public Task<NoteEntity?> ArchiveNoteAsync(NoteIdRequest noteId, int userId)
        {
            throw new NotImplementedException();
        }
        public Task<NoteEntity?> TrashUnTrashNoteAsync(NoteIdRequest noteId, int userId){
            throw new NotImplementedException();
        }
    }
}
