using BusinessLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using ModelLayer.Request.NoteRequest;
using ModelLayer.Response.NoteResponse;
using ModelLayer.Response.UserResponse;
using System.Security.Claims;

namespace FunDo.Controllers
{
    [Authorize] // This is called filter
    [ApiController]
    [Route("api/[controller]")]
    public class NoteController: ControllerBase
    {
        private readonly ILogger<NoteController> _logger;
        private readonly INoteBL _noteBL;
        private const string UserIdClaim = ClaimTypes.NameIdentifier;
        public NoteController(ILogger<NoteController> logger, INoteBL noteBL)
        {
            _logger = logger;
            _noteBL = noteBL;
        }

        [HttpPost("create-note")]
        public async Task<ActionResult<ResponseModel<NoteModelResponse>>> AddNote([FromBody]NoteCreationRequest note)
        {
            if (!TryGetUserId(out int userId))
            {
                _logger.LogWarning("AddNote rejected: user id claim missing or invalid in token");
                return Unauthorized();
            }
            _logger.LogInformation("AddNote started for user {UserId}", userId);

            try
            {
                var result = await _noteBL.AddNoteAsync(note, userId);

                if (!result.Success)
                {
                    _logger.LogWarning("AddNote failed for user {UserId}: BL returned Success=false", userId);
                    return BadRequest(result);
                }

                _logger.LogInformation("AddNote succeeded for user {UserId}", userId);
                return StatusCode(StatusCodes.Status201Created, result);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "AddNote crashed unexpectedly for user {UserId}", userId);
                return StatusCode(StatusCodes.Status500InternalServerError, "Something went wrong while creating the note.");
            }


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

        private bool TryGetUserId(out int userId)
        {
            return int.TryParse(User.FindFirstValue(UserIdClaim), out userId);
        }
    }
}
