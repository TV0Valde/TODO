using FluentValidation;

namespace TODO.Application.Notes.Commands.DeleteNote
{
    public class DeleteNoteCommandValidator:AbstractValidator<DeleteNoteCommand>
    {
        public DeleteNoteCommandValidator() { 
            RuleFor(x=>x.Id).NotEmpty();
        }
    }
}
