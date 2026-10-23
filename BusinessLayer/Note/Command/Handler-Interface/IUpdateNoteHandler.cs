using ModelLayer.Response.NoteResponse;
using ModelLayer.Response.UserResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Note.Command.Handler_Interface
{
    public interface IUpdateNoteHandler
    {
        Task<ResponseModel<NoteModelResponse>> HandleAsync(UpdateNoteCommand command);

    }
}
