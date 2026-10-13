using BusinessLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using ModelLayer.Request.NoteRequest;
using ModelLayer.Response;
using ModelLayer.Response.NoteResponse;

namespace FunDo.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NoteController: ControllerBase
    {
        private readonly INoteBL _noteBL;
        public NoteController(INoteBL noteBL)
        {
            _noteBL = noteBL;
        }

        [HttpPost("add-note")]
        public Task<ActionResult<ResponseModel<NoteModelResponse>>> AddNote([FromBody]NoteCreationRequest note)
        {
            //if(!TryGetUserId(out int userId))
            //{
            //    return Unauthorized(new ResponseModel<NoteModelResponse>
            //    {
            //        Success = false,
            //        Message = "Invalid user identity",
            //        Data = null
            //    });
            //}
            //var result = await _noteBL.AddNoteAsync(note, userId);

            //if (!result.Success)
            //{
            //    return BadRequest(result);
            //}

            //return StatusCode(StatusCodes.Status201Created, result);
            throw new NotImplementedException();

        }

        [HttpGet("view-note")]
        public Task<ActionResult<ResponseModel<NoteModelResponse>>> ViewNote()
        {
            throw new NotImplementedException();

        }

        [HttpGet("view-note/{user_id}")]
        public Task<ActionResult<ResponseModel<NoteModelResponse>>> ViewNoteById(NoteIdRequest note)
        {
            throw new NotImplementedException();

        }

        [HttpGet("edit-note/{user_id}")]
        public Task<ActionResult<ResponseModel<NoteModelResponse>>> EditNoteById(EditNoteRequest note)
        {
            throw new NotImplementedException();

        }
        public Task<ActionResult<ResponseModel<NoteModelResponse>>> DeleteNoteById(NoteIdRequest note)
        {
            throw new NotImplementedException();

        }
        public Task<ActionResult<ResponseModel<NoteModelResponse>>> ArchieveNote()
        {
            throw new NotImplementedException();

        }
        public Task<ActionResult<ResponseModel<NoteModelResponse>>> TrashUnTrash()
        {
            throw new NotImplementedException();

        }

    }
}
