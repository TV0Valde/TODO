using MediatR;

namespace TODO.Application.Notes.Commands.SetNoteTagsCommand
{
    public record SetNoteTagsCommand(int NoteId, List<int> TagIds) : IRequest<Unit>;
}
