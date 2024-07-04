using MediatR;
using TODO.Persistence.Interfaces;

namespace TODO.Application.Reminders.Commands.DeleteReminder
{
    public class DeleteReminderCommandHandler : IRequestHandler<DeleteReminderCommand,bool>
    {
        private readonly IReminderRepository _repository;
        public DeleteReminderCommandHandler(IReminderRepository repository) {
            _repository = repository;
        }
        public async Task<bool> Handle(DeleteReminderCommand request, CancellationToken cancellationToken) {

            var reminder = await _repository.GetByIdAsync(request.Id);
            if (reminder == null) { 
             return false;
            }
            _repository.Delete(request.Id);
            await _repository.SaveAsync();
            return true;
        
        }
    }
}
