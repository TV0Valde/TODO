using MediatR;

namespace TODO.Application.Notes.Commands.DeleteNote
{
    public record DeleteNoteCommand(int Id) : IRequest<bool>;
}
