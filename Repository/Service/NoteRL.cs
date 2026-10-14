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
                Colour = note.Colour,
                Description = note.Description,
                UserId = userId
            };

            await _fundoocontext.AddAsync(new_note);
            await _fundoocontext.SaveChangesAsync();
            return new_note;

        }
        public async Task<List<NoteEntity>> ViewNotesAsync(int userId)
        {
            var user_exists = await _fundoocontext.Users.AnyAsync(x => x.UserId == userId);
            if (!user_exists)
            {
                throw new KeyNotFoundException("User doesn't exist");
            }
            var notes = await _fundoocontext.Notes.Where(x => x.UserId == userId && !x.IsDeleted).ToListAsync();
            return notes;
        }
        public async Task<NoteEntity?> ViewNotesByIdAsync(NoteIdRequest note, int userId)
        {
            var user_exists = await _fundoocontext.Users.AnyAsync(x => x.UserId == userId);
            if (!user_exists)
            {
                throw new KeyNotFoundException("User Not Found");
            }
            var note_ = await _fundoocontext.Notes.FirstOrDefaultAsync(x => x.UserId == userId && !x.IsDeleted && x.NoteId == note.NoteId);
            return note_;
        }
        public async Task<NoteEntity?> EditNotesByIdAsync(EditNoteRequest note, int userId)
        {
            var user_exists = await _fundoocontext.Users.AnyAsync(x => x.UserId == userId);
            if (!user_exists)
            {
                throw new KeyNotFoundException("User Not Found");
            }
            var note_ = await _fundoocontext.Notes.FirstOrDefaultAsync(x => x.UserId == userId && !x.IsDeleted && x.NoteId == note.NoteId);
            if (note_ == null)
            {
                throw new KeyNotFoundException("Note Not Found");
            }
            note_.Title = note.Title;
            note_.Colour = note.Colour;
            note_.Description = note.Description;
            await _fundoocontext.SaveChangesAsync();
            return note_;
        }
        public async Task<NoteEntity?> DeleteNotesByIdAsync(NoteIdRequest note, int userId)
        {
            var user_exists = await _fundoocontext.Users.AnyAsync(x => x.UserId == userId);
            if (!user_exists)
            {
                throw new KeyNotFoundException("User Not Found");
            }
            var note_ = await _fundoocontext.Notes.FirstOrDefaultAsync(x => x.UserId == userId && !x.IsDeleted && x.NoteId == note.NoteId);
            if (note_ == null)
            {
                return null;
            }

            note_.IsDeleted = true;
            await _fundoocontext.SaveChangesAsync();
            return note_;
        }
        public async Task<NoteEntity?> ArchiveNoteAsync(NoteIdRequest noteId, int userId)
        {
            var user_exists = await _fundoocontext.Users.AnyAsync(x => x.UserId == userId);
            if (!user_exists)
            {
                throw new KeyNotFoundException("User Not Found");
            }
            var note_ = await _fundoocontext.Notes.FirstOrDefaultAsync(x => x.UserId == userId && !x.IsDeleted && x.NoteId == noteId.NoteId);
            if (note_ == null)
            {
                return null;
            }

            note_.IsArchived = true;
            await _fundoocontext.SaveChangesAsync();
            return note_;
        }
       
    }
}
