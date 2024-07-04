using MediatR;
using TODO.Persistence.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;

namespace TODO.Application.Notes.Commands.DeleteNote
{
    public class DeleteNoteCommandHandler : IRequestHandler<DeleteNoteCommand,bool>
    {   
        private readonly INoteRepository _repository;
        private readonly IValidator<DeleteNoteCommand> _validator;
        public DeleteNoteCommandHandler(INoteRepository repository, IValidator<DeleteNoteCommand> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public  async Task<bool> Handle(DeleteNoteCommand request, CancellationToken cancellationToken)
        {
           await  _validator.ValidateAndThrowAsync(request, cancellationToken);
            var note = await _repository.GetByIdAsync(request.Id);
            if (note == null) {
                return false;
            }
            _repository.Delete(request.Id);
            await _repository.SaveAsync();
            return true;
        }
    }
}