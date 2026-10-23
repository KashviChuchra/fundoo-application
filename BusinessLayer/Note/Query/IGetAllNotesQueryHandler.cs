using ModelLayer.Response.NoteResponse;
using ModelLayer.Response.UserResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Note.Query
{
    public interface IGetAllNotesQueryHandler
    {
        Task<ResponseModel<List<NoteModelResponse>>> HandleAsync(GetAllNotesQuery query);
    }
}
