using FluentValidation;
using MediatR;
using TODO.Application.Common.Exceptions;
using TODO.Persistence.Interfaces;

using TODO.Core.Models;
using TODO.Application.Reminders.Commands.UpdateReminder;

namespace TODO.Application.Reminders.Commands.UpdateReminder
{
    public class UpdateReminderCommandHandler : IRequestHandler<UpdateReminderCommand, Reminder>
    {
        private readonly IReminderRepository _repository;
        private readonly IValidator<UpdateReminderCommand> _validator;

        public UpdateReminderCommandHandler(IReminderRepository repository,IValidator<UpdateReminderCommand> validator)
        {
            _repository = repository;
            _validator = validator;
        }
        public async Task<Reminder> Handle(UpdateReminderCommand request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);
            if (_repository.TitleExist(request.Title)) { 
                throw new DuplecateTitleException(request.Title);
            }
            var reminder = await _repository.GetByIdAsync(request.Id);
            if (reminder == null)
            {
                throw new NotFoundException(nameof(Note), request.Id);
            }
            reminder.Title = request.Title;
            reminder.Description = request.Description;
            reminder.Reminder_time = request.Reminder_time;

            _repository.Update(reminder);
            await _repository.SaveAsync();

            return reminder;
        }

    }
}
