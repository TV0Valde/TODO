using MediatR;
using TODO.Application.Interfaces;
using TODO.Core.Models;


namespace TODO.Application.Tags.Queries.GetAllTagsQuery
{
    public class GetAllTagsQueryHandler : IRequestHandler<GetAllTagsQuery, IEnumerable<Tag>>
    {
        private readonly ITagRepository _tagRepository;

        public GetAllTagsQueryHandler(ITagRepository tagRepository)
        {
            _tagRepository = tagRepository;
        }

        public async Task<IEnumerable<Tag>> Handle(GetAllTagsQuery request, CancellationToken cancellationToken)
        {
            return await _tagRepository.GetAllAsync();
        }
    }
}
