using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TODO.Application.Common.Exceptions;
using TODO.Application.Interfaces;
using TODO.Core.Models;
using TODO.Application.Notes.Commands.CreateNote;

namespace TODO.Application.Notes.Commands.UpdateNote
{
    public class UpdateNoteCommandHandler: IRequestHandler<UpdateNoteCommand,Note>
    {
        private readonly INoteRepository _noteRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IValidator<UpdateNoteCommand> _validator;

        public UpdateNoteCommandHandler(INoteRepository noteRepository, ITagRepository tagRepository, IValidator<UpdateNoteCommand> validator) {
            _noteRepository = noteRepository;
            _tagRepository = tagRepository;
            _validator = validator;
        }
        public async Task<Note> Handle(UpdateNoteCommand request,CancellationToken cancellationToken) {
            await _validator.ValidateAndThrowAsync(request, cancellationToken);

            if (_noteRepository.TitleExist(request.Title))
            {
                throw new DuplecateTitleException(request.Title);
            }
            var note = await _noteRepository.GetByIdAsync(request.Id);
            if (note == null) {
                throw new NotFoundException(nameof(Note), request.Id);
            }
            note.Title = request.Title;
            note.Description = request.Description;
            note.Tags = request.Tags;

            _noteRepository.Update(note);
          await _noteRepository.SaveAsync();

            return note;
        }
        
    }
}
