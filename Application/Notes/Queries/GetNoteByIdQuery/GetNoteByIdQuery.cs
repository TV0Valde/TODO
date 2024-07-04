using TODO.Core.Models;
using MediatR;

namespace TODO.Application.Notes.Queries.GetNoteByIdQuery
{
    public record GetNoteByIdQuery(int Id) : IRequest<Note>;
    
}
