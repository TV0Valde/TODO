using MediatR;
using Microsoft.AspNetCore.Mvc;
using TODO.Application.Reminders.Commands.CreateReminder;
using TODO.Application.Reminders.Commands.DeleteReminder;
using TODO.Application.Reminders.Commands.UpdateReminder;
using TODO.Application.Reminders.Queries.GetAllRemindersQuery;
using TODO.Application.Reminders.Queries.GetReminderByIdQuery;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TODO.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RemindersController : ControllerBase
    {
        private readonly IMediator _mediator;
        public RemindersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] CreateReminderCommand command)
        {
            var reminder = await _mediator.Send(command);
            if (reminder == null)
            {
                return BadRequest("Ошибка при создании напоминания.");
            }

            return StatusCode(201, reminder);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteReminderCommand(id));
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateReminderCommand command)
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
        public async Task<IActionResult> GetById(int id) {
            var reminder = await _mediator.Send(new GetReminderByIdQuery(id));
            if (reminder == null) {
                return NotFound();
            }
            return Ok(reminder);
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll()
        {
            var reminders = await _mediator.Send(new GetAllRemindersQuery());
            return Ok(reminders);
        }


    }
}
