using MediatR;
using TODO.Core.Models;

namespace TODO.Application.Reminders.Queries.GetAllRemindersQuery
{
    public class GetAllRemindersQuery : IRequest<IEnumerable<Reminder>>;
}
