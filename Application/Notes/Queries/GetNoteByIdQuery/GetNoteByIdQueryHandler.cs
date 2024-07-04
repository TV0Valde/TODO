using MediatR;
using TODO.Application.Common.Exceptions;
using TODO.Persistence.Interfaces;

using TODO.Core.Models;

namespace TODO.Application.Notes.Queries.GetNoteByIdQuery
{
    public class GetNoteByIdQueryHandler : IRequestHandler<GetNoteByIdQuery,Note>
    {
        private readonly INoteRepository _repository;
        public GetNoteByIdQueryHandler(INoteRepository repository) {
            _repository = repository;
        }

        public async Task<Note> Handle(GetNoteByIdQuery request, CancellationToken cancellationToken) {
            var note = await _repository.GetByIdAsync(request.Id);
            if (note == null) {
                throw new NotFoundException(nameof(Note), request.Id);
            }
            return note;
        }

    }
}
