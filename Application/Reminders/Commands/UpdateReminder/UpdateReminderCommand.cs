using MediatR;
using TODO.Core.Models;

namespace TODO.Application.Reminders.Commands.UpdateReminder
{
    public record UpdateReminderCommand(int Id, string Title, string Description, DateTime Reminder_time) : IRequest<Reminder>;
  
}
