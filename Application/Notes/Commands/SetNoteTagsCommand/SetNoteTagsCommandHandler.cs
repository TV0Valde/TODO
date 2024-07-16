using MediatR;
using TODO.Application.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace TODO.Application.Notes.Commands.SetNoteTagsCommand
{
    public class SetNoteTagsCommandHandler : IRequestHandler<SetNoteTagsCommand,Unit>
    {
        private readonly INoteRepository _noteRepository;

        public SetNoteTagsCommandHandler(INoteRepository noteRepository)
        {
            _noteRepository = noteRepository;
        }

        public async Task<Unit> Handle(SetNoteTagsCommand request, CancellationToken cancellationToken)
        {
            await _noteRepository.SetTagsAsync(request.NoteId, request.TagIds);
            return Unit.Value;
        }
    }
}
