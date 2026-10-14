using BusinessLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using ModelLayer.Request.NoteRequest;
using ModelLayer.Response;
using ModelLayer.Response.NoteResponse;
using System.Security.Claims;

namespace FunDo.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class NoteController: ControllerBase
    {
        private readonly INoteBL _noteBL;
        private const string UserIdClaim = ClaimTypes.NameIdentifier;
        public NoteController(INoteBL noteBL)
        {
            _noteBL = noteBL;
        }

        [HttpPost("create-note")]
        public async Task<ActionResult<ResponseModel<NoteModelResponse>>> AddNote([FromBody]NoteCreationRequest note)
        {
            if (!TryGetUserId(out int userId))
            {
                return Unauthorized();
            }
            var result = await _noteBL.AddNoteAsync(note, userId);

            if (!result.Success)
            {
                return BadRequest(result);
            }

            return StatusCode(StatusCodes.Status201Created, result);
        }

        [HttpGet("view-notes")]
        public async Task<ActionResult<ResponseModel<List<NoteModelResponse>>>> ViewNote()
        {
            if (!TryGetUserId(out var userId))
            {
                return Unauthorized(); 
            }
            var result = await _noteBL.ViewNotesAsync(userId);
            return Ok(result);
        }

        [HttpGet("view-note")]
        public async Task<ActionResult<ResponseModel<NoteModelResponse>>> ViewNoteById([FromQuery] int noteId)
        {
            if (!TryGetUserId(out var userId))
            {
                return Unauthorized();
            }
            var note = new NoteIdRequest { NoteId = noteId };

            var result = await _noteBL.ViewNotesByIdAsync(note, userId);

            if (!result.Success)    return NotFound(result);

            return Ok(result);
        }

        [HttpPut("edit-note")]
        public async Task<ActionResult<ResponseModel<NoteModelResponse>>> EditNoteById([FromBody]EditNoteRequest note)
        {
            if (!TryGetUserId(out var userId))
            {
                return Unauthorized();
            }
            var result = await _noteBL.EditNotesByIdAsync(note, userId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);

        }
        [HttpDelete("delete-note")]
        public async Task<ActionResult<ResponseModel<NoteModelResponse>>> DeleteNoteById([FromBody]NoteIdRequest note)
        {
            if (!TryGetUserId(out var userId))
            {
                return Unauthorized();
            }
            var result = await _noteBL.DeleteNotesByIdAsync(note, userId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }
        
        [HttpPut("archive-note")]
        public async Task<ActionResult<ResponseModel<NoteModelResponse>>> ArchieveNoteById([FromBody] NoteIdRequest note)
        {
            if (!TryGetUserId(out var userId))
            {
                return Unauthorized();
            }
            var result = await _noteBL.ArchiveNoteAsync(note, userId);

            if (!result.Success)
                return NotFound(result);

            return Ok(result);
        }

        //public Task<ActionResult<ResponseModel<NoteModelResponse>>> TrashUnTrash()
        //{
        //    throw new NotImplementedException();

        //}

        private bool TryGetUserId(out int userId)
        {
            return int.TryParse(User.FindFirstValue(UserIdClaim), out userId);
        }
    }
}
