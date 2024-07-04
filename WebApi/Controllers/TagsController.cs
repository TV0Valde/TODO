using MediatR;
using Microsoft.AspNetCore.Mvc;
using TODO.Application.Notes.Commands.SetNoteTagsCommand;
using TODO.Application.Reminders.Commands.SetReminderTagsCommand;
using TODO.Application.Tags.Commands.CreateTagCommand;
using TODO.Application.Tags.Commands.DeleteTagCommand;
using TODO.Application.Tags.Commands.UpdateTagCommand;
using TODO.Application.Tags.Queries.GetAllTagsQuery;
using TODO.Application.Tags.Queries.GetTagByIdQuery;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TODO.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly IMediator _mediator;
        public TagsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateTagCommand command)
        {
            var tag = await _mediator.Send(command);
            if (tag == null)
            {
                return BadRequest("Ошибка при создании тэга.");
            }

            return StatusCode(201, tag);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteTagCommand(id));
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTagCommand command)
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
            var tag = await _mediator.Send(new GetTagByIdQuery(id));
            if (tag == null)
            {
                return NotFound();
            }
            return Ok(tag);
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var tag = await _mediator.Send(new GetAllTagsQuery());

            return Ok(tag);
        }
       
        [HttpPost("setTagsForNote")]
        public async Task<IActionResult> SetTagsForNote(int id, [FromBody] List<int> tagIds)
        {
           await _mediator.Send(new SetNoteTagsCommand(id, tagIds));   
            return NoContent();
        }

        [HttpPost("setTagsForReminders")]
        public async Task<IActionResult> SetTagsForReminders(int id, [FromBody] List<int> tagIds)
        {
            await _mediator.Send(new SetReminderTagsCommand(id, tagIds));
            return NoContent();
        }
    }
}
