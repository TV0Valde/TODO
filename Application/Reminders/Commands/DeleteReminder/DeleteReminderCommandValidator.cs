using FluentValidation;
using TODO.Application.Reminders.Commands.DeleteReminder;

namespace TODO.Application.Reminders.Commands.CreateReminder
{
    public class DeleteReminderCommandValidator : AbstractValidator<DeleteReminderCommand>
    {
        public DeleteReminderCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty();
        }
    }
}
