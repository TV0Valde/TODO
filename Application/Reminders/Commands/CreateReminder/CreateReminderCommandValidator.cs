using FluentValidation;
using TODO.Application.Interfaces;

namespace TODO.Application.Reminders.Commands.CreateReminder
{
    public class CreateReminderCommandValidator : AbstractValidator<CreateReminderCommand>
    {
        private readonly IReminderRepository _reminderRepository;
        public CreateReminderCommandValidator(IReminderRepository reminderRepository)
        {
            _reminderRepository = reminderRepository;
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Название обязательно!")
                .MaximumLength(50).WithMessage("Название не может быть длинее 50 символов!");
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Напоминание не может быть пустым!")
                .MaximumLength(250).WithMessage("Максимальная длина напоминания 250 символов!");

         


        }
       


    }
}
