using MediatR;
using TODO.Application.Common.Exceptions;
using TODO.Application.Interfaces;
using TODO.Core.Models;

namespace TODO.Application.Reminders.Queries.GetReminderByIdQuery
{
    public class GetReminderByIdQueryHandler : IRequestHandler<GetReminderByIdQuery,Reminder>
    {
        private readonly IReminderRepository _repository;

        public GetReminderByIdQueryHandler(IReminderRepository repository)
        {
            _repository = repository;
        }

        public async Task<Reminder> Handle(GetReminderByIdQuery request, CancellationToken cancellationToken) {
            var reminder = await _repository.GetByIdAsync(request.Id);
            if (reminder == null) {
                throw new NotFoundException(nameof(reminder), request.Id);
            }
            return reminder;
        }
    }
}
