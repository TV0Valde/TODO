using MediatR;
using TODO.Application.Common.Exceptions;
using TODO.Application.Interfaces;
using TODO.Core.Models;

namespace TODO.Application.Tags.Queries.GetTagByIdQuery
{
    public class GetTagByIdQueryHandler : IRequestHandler<GetTagByIdQuery, Tag>
    {
        private readonly ITagRepository _repository;

        public GetTagByIdQueryHandler(ITagRepository repository)
        {
            _repository = repository;
        }

        public async Task<Tag> Handle(GetTagByIdQuery request, CancellationToken cancellationToken)
        {
            var tag = await _repository.GetByIdAsync(request.Id);
            if (tag == null)
            {
                throw new NotFoundException(nameof(tag), request.Id);
            }
            return tag;
        }
    }
}
