using FluentValidation;
using MediatR;
using TODO.Application.Common.Exceptions;
using TODO.Application.Interfaces;
using TODO.Core.Models;

namespace TODO.Application.Tags.Commands.CreateTagCommand
{
    public class CreateTagCommandHandler : IRequestHandler<CreateTagCommand, Tag>
    {
        private readonly ITagRepository _repository;
        private readonly IValidator<CreateTagCommand> _validator;
        public CreateTagCommandHandler(ITagRepository repository, IValidator<CreateTagCommand> validator)
        {
            _repository = repository;
            _validator = validator;
        }
        public async Task<Tag> Handle(CreateTagCommand request, CancellationToken cancellationToken)
        {
          await  _validator.ValidateAndThrowAsync(request, cancellationToken);
            if (_repository.NameExist(request.Name)) {
                throw new DuplecateTitleException(request.Name);
            }
            var tag = await _repository.CreateAsync(new Tag { Name = request.Name });
            return tag;
        }
    }
}
