using FluentValidation;
using MediatR;
using TODO.Persistence.Interfaces;
using TODO.Core.Models;
using TODO.Application.Notes.Commands.CreateNote;
using TODO.Application.Common.Exceptions;

namespace TODO.Application.Notes.Commands
{
    public class CreateNoteCommandHandler : IRequestHandler<CreateNoteCommand, Note>
    {
        private readonly INoteRepository _noteRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IValidator<CreateNoteCommand> _validator;

        public CreateNoteCommandHandler(INoteRepository noteRepository, ITagRepository tagRepository, IValidator<CreateNoteCommand> validator)
        {
            _noteRepository = noteRepository;
            _tagRepository = tagRepository;
            _validator = validator;
        }

        public async Task<Note> Handle(CreateNoteCommand request, CancellationToken cancellationToken)
        {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);

            if (_noteRepository.TitleExist(request.Title))
            {
                throw new DuplecateTitleException(request.Title);
            }

            var tags = new List<Tag>();

            foreach (var tagName in request.Tags)
            {
                var tag = await _tagRepository.GetByNameAsync(tagName);
                if (tag == null)
                {
                    tag = new Tag { Name = tagName };
                    await _tagRepository.CreateAsync(tag);
                }
                tags.Add(tag);
            }

            Note note = await _noteRepository.CreateAsync(new Note { Title = request.Title, Description = request.Description, Tags = tags });
            return note;
        }
    }
}
