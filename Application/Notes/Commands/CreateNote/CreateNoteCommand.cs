using MediatR;
using TODO.Core.Models;

namespace TODO.Application.Notes.Commands.CreateNote
{
    public record CreateNoteCommand(string Title, string Description, List<string> Tags) : IRequest<Note>;

}
