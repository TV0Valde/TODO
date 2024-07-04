using MediatR;
using TODO.Core.Models;
namespace TODO.Application.Notes.Commands.UpdateNote
{
    public record UpdateNoteCommand(int Id, string Title, string Description, List<Tag>Tags) : IRequest<Note>;
    
}
