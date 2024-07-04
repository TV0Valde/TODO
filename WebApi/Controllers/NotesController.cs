using MediatR;
using Microsoft.AspNetCore.Mvc;
using TODO.Application.Notes.Commands.CreateNote;
using TODO.Application.Notes.Commands.DeleteNote;
using TODO.Application.Notes.Commands.UpdateNote;
using TODO.Application.Notes.Queries.GetNoteByIdQuery;
using TODO.Application.Notes.Queries.GetAllNotesQuery;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TODO.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly IMediator _mediator;
        public NotesController(IMediator mediator) {
            _mediator = mediator;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateNoteCommand command)
        {
            var note = await _mediator.Send(command);
            if (note == null)
            {
                return BadRequest("Ошибка при создании записи.");
            }
           
            return StatusCode(201,note);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(int id)
        {

            var result = await _mediator.Send(new DeleteNoteCommand(id));
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateNoteCommand command)
        {

            if (id != command.Id)
            {
                return BadRequest();
            }
            var result = await _mediator.Send(command);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetById(int id)
        {
            var note = await _mediator.Send(new GetNoteByIdQuery(id));
            if (note == null)
            {
                return NotFound();
            }
            return Ok(note);
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var notes = await _mediator.Send(new GetAllNotesQuery());
            return Ok(notes);
        }

       
       
       


    }
}
