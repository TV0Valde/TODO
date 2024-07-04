using MediatR;
using TODO.Persistence.Interfaces;
using TODO.Core.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TODO.Application.Notes.Queries.GetAllNotesQuery
{
    public class GetAllNotesQueryHandler : IRequestHandler<GetAllNotesQuery, IEnumerable<Note>>
    {
        private readonly INoteRepository _noteRepository;
        private readonly ITagRepository _tagRepository;

        public GetAllNotesQueryHandler(INoteRepository noteRepository, ITagRepository tagRepository)
        {
            _noteRepository = noteRepository;
            _tagRepository = tagRepository;
        }

        public async Task<IEnumerable<Note>> Handle(GetAllNotesQuery request, CancellationToken cancellationToken)
        {
            return await _noteRepository.GetAllAsync();
        }
    }
}
