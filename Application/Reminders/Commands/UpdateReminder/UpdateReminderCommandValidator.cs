﻿using FluentValidation;
namespace TODO.Application.Reminders.Commands.UpdateReminder
{
    public class UpdateReminderCommandValidator : AbstractValidator<UpdateReminderCommand>
    {
        public UpdateReminderCommandValidator()
        {
            RuleFor(x => x.Title)
                    .NotEmpty().WithMessage("Название обязательно!")
                    .MaximumLength(50).WithMessage("Название не может быть длинее 50 символов!");
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Напоминание не может быть пустым!")
                .MaximumLength(250).WithMessage("Максимальная длина напоминания 250 символов!");
        }
    }
}
