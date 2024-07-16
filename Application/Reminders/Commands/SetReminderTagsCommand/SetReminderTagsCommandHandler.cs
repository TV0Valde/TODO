using MediatR;
using TODO.Application.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace TODO.Application.Reminders.Commands.SetReminderTagsCommand
{
    public class SetReminderTagsCommandHandler : IRequestHandler<SetReminderTagsCommand,Unit>
    {
        private readonly IReminderRepository _reminderRepository;

        public SetReminderTagsCommandHandler(IReminderRepository reminderRepository)
        {
            _reminderRepository = reminderRepository;
        }

        public async Task<Unit> Handle(SetReminderTagsCommand request, CancellationToken cancellationToken)
        {
            await _reminderRepository.SetTagsAsync(request.ReminderId, request.TagIds);
            return Unit.Value;
        }
    }
}
