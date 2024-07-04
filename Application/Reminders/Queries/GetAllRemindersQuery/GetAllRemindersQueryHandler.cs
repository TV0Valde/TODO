using MediatR;
using TODO.Persistence.Interfaces;
using TODO.Core.Models;

namespace TODO.Application.Reminders.Queries.GetAllRemindersQuery
{
    public class GetAllRemindersQueryHandler : IRequestHandler<GetAllRemindersQuery,IEnumerable<Reminder>>
    {
        private readonly IReminderRepository _repository;
        public GetAllRemindersQueryHandler(IReminderRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Reminder>> Handle(GetAllRemindersQuery request, CancellationToken cancellationToken) {
            return await _repository.GetAllAsync();
        }
    }
}
