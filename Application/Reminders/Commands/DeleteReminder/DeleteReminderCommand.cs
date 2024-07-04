using MediatR;

namespace TODO.Application.Reminders.Commands.DeleteReminder
{
    public record DeleteReminderCommand(int Id) : IRequest<bool>;
}
