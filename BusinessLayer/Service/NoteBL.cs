using BusinessLayer.Interface;
using BusinessLayer.Note.Command;
using BusinessLayer.Note.Handler;
using BusinessLayer.Note.Query;
using ModelLayer.Request.NoteRequest;
using ModelLayer.Response.NoteResponse;
using ModelLayer.Response.UserResponse;
using RepositoryLayer.Interface;
using BusinessLayer.Note.Command.Handler_Interface;


namespace BusinessLayer.Service
{
    public class NoteBL: INoteBL
    {
        private readonly INoteRL _noteRL;
        private readonly ICreateNoteHandler _createNoteHandler;
        private readonly IUpdateNoteHandler _updateNoteHandler;
        private readonly IDeleteNoteHandler _deleteNoteHandler;
        private readonly IArchiveNoteHandler _archiveNoteHandler;
        private readonly IGetAllNotesQueryHandler _getAllNotesQueryHandler;
        private readonly IGetNotesByIdQueryHandler _getNotesByIdQueryHandler;



        public NoteBL(INoteRL noteRL, 
            ICreateNoteHandler createNoteHandler, IUpdateNoteHandler updateNoteHandler, 
            IDeleteNoteHandler deleteNoteHandler, IArchiveNoteHandler archiveNoteHandler,
            IGetAllNotesQueryHandler getAllNotesQueryHandler,IGetNotesByIdQueryHandler getNotesByIdQueryHandler)
        {
            _noteRL = noteRL;
            _createNoteHandler = createNoteHandler;
            _updateNoteHandler = updateNoteHandler;
            _deleteNoteHandler = deleteNoteHandler;
            _archiveNoteHandler = archiveNoteHandler;
            _getAllNotesQueryHandler = getAllNotesQueryHandler;
            _getNotesByIdQueryHandler = getNotesByIdQueryHandler;
        }

        public async Task<ResponseModel<NoteModelResponse>> AddNoteAsync(NoteCreationRequest note, int userId)
        {
            var command = new CreateNoteCommand(note, userId);
            return await _createNoteHandler.HandleAsync(command);
        }
        public async Task<ResponseModel<List<NoteModelResponse>>> ViewNotesAsync(int userId)
        {
            var query = new GetAllNotesQuery(userId);
            return await _getAllNotesQueryHandler.HandleAsync(query);
        }
        public async Task<ResponseModel<NoteModelResponse>> ViewNotesByIdAsync(NoteIdRequest note, int userId)
        {
            var query = new GetNotesByIdQuery(note, userId);
            return await _getNotesByIdQueryHandler.HandleAsync(query);
        }
        public async Task<ResponseModel<NoteModelResponse>> EditNotesByIdAsync(EditNoteRequest note, int userId)
        {
            var command = new UpdateNoteCommand(note, userId);
            return await _updateNoteHandler.HandleAsync(command);
        }
        public async Task<ResponseModel<NoteModelResponse>> DeleteNotesByIdAsync(NoteIdRequest note, int userId)
        {
            var command = new DeleteNoteCommand(note, userId);
            return await _deleteNoteHandler.HandleAsync(command);
        }
        public async Task<ResponseModel<NoteModelResponse>> ArchiveNoteAsync(NoteIdRequest noteId, int userId)
        {
            var command = new ArchiveNoteCommand(noteId, userId);
            return await _archiveNoteHandler.HandleAsync(command);
        }
    }
}
