using MediatR;
using TODO.Core.Models;
namespace TODO.Application.Reminders.Commands.CreateReminder
{
    public record CreateReminderCommand(string Title, string Description, DateTime Reminder_time, List<string> Tags) : IRequest<Reminder>;
    
}
