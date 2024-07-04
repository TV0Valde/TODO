using MediatR;
using TODO.Core.Models;

namespace TODO.Application.Reminders.Queries.GetReminderByIdQuery
{
    public record GetReminderByIdQuery(int Id) : IRequest<Reminder>;
}
