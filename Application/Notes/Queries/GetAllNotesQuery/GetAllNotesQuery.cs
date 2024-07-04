using MediatR;
using TODO.Core.Models;
namespace TODO.Application.Notes.Queries.GetAllNotesQuery
{
    public record GetAllNotesQuery : IRequest<IEnumerable<Note>>;
    
}
