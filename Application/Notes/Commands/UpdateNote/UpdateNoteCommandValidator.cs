using FluentValidation;
namespace TODO.Application.Notes.Commands.UpdateNote
{
    public class UpdateNoteCommandValidator : AbstractValidator<UpdateNoteCommand>
    {
        public UpdateNoteCommandValidator() {
            RuleFor(x => x.Title)
                    .NotEmpty().WithMessage("Название обязательно!")
                    .MaximumLength(50).WithMessage("Название не может быть длинее 50 символов!");
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Заметка не может быть пустой.")
                .MaximumLength(250).WithMessage("Максимальная длина заметки 250 символов");
        }
    }
}
