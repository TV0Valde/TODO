using FluentValidation;
using MediatR;
using TODO.Application.Common.Exceptions;
using TODO.Persistence.Interfaces;
using TODO.Core.Models;

namespace TODO.Application.Reminders.Commands.CreateReminder
{
    public class CreateReminderCommandHandler : IRequestHandler<CreateReminderCommand,Reminder>
    {
        private readonly IReminderRepository _reminderRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IValidator<CreateReminderCommand> _validator;
        public CreateReminderCommandHandler(IReminderRepository reminderRepository, ITagRepository tagRepository, IValidator<CreateReminderCommand> validator)
        {
            _reminderRepository = reminderRepository;
            _tagRepository = tagRepository;
            _validator = validator;
        }
        public async Task<Reminder> Handle(CreateReminderCommand request, CancellationToken cancellationToken) {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);
            if (_reminderRepository.TitleExist(request.Title)) {
                throw new DuplecateTitleException(request.Title);
            }
            var tags = new List<Tag>();

            foreach (var tagName in request.Tags)
            {
                var tag = await _tagRepository.GetByNameAsync(tagName);
                if (tag == null)
                {
                    tag = new Tag { Name = tagName };

                    await _tagRepository.CreateAsync(tag);
                }
                tags.Add(tag);
            }
            Reminder reminder = await _reminderRepository.CreateAsync(new Reminder { Title = request.Title, Description = request.Description,Reminder_time =request.Reminder_time, Tags = tags });
            return reminder;
        }
    }
}
