using MediatR;

namespace TODO.Application.Reminders.Commands.SetReminderTagsCommand
{
    public record SetReminderTagsCommand(int ReminderId, List<int> TagIds) : IRequest<Unit>;
}
