using MediatR;
using TODO.Application.Common.Exceptions;
using TODO.Persistence.Interfaces;
using TODO.Core.Models;


namespace TODO.Application.Tags.Commands.UpdateTagCommand
{
    public class UpdateTagCommandHandler : IRequestHandler<UpdateTagCommand, Tag>
    {
        private readonly ITagRepository _repository;

        public UpdateTagCommandHandler(ITagRepository repository)
        {
            _repository = repository;
        }
        public async Task<Tag> Handle(UpdateTagCommand request, CancellationToken cancellationToken)
        {
            var tag = await _repository.GetByIdAsync(request.Id);
            if (tag == null)
            {
                throw new NotFoundException(nameof(Tag), request.Id);
            }
            tag.Name = request.Name;

            _repository.Update(tag);
            await _repository.SaveAsync();

            return tag;
        }

    }
}
