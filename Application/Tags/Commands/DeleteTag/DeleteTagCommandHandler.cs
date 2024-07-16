using MediatR;
using TODO.Application.Interfaces;
using TODO.Core.Models;

namespace TODO.Application.Tags.Commands.DeleteTagCommand
{
    public class DeleteTagCommandHandler : IRequestHandler<DeleteTagCommand,bool>
    {
        private readonly ITagRepository _repository;
        public DeleteTagCommandHandler(ITagRepository repository) {
            _repository = repository;
        }
        public async Task<bool> Handle(DeleteTagCommand request, CancellationToken cancellationToken)
        {
            var tag = await _repository.GetByIdAsync(request.Id);
            if (tag == null)
            {
                return false;
            }
            _repository.Delete(request.Id);
            await _repository.SaveAsync();
            return true;
        }


    }
}
