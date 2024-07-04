using FluentValidation;
using TODO.Persistence.Interfaces;
namespace TODO.Application.Notes.Commands.CreateNote
{
    public class CreateNoteCommandValidator :AbstractValidator<CreateNoteCommand>
    {
        private readonly INoteRepository _noteRepository;

       

        public CreateNoteCommandValidator(INoteRepository noteRepository) {
            _noteRepository = noteRepository;
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Название обязательно!")
                .MaximumLength(50).WithMessage("Название не может быть длинее 50 символов!");
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Заметка не может быть пустой.")
                .MaximumLength(250).WithMessage("Максимальная длина заметки 250 символов");



        }
       

    }
}
