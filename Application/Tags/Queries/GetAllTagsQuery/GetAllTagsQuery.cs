using MediatR;
using TODO.Core.Models;

namespace TODO.Application.Tags.Queries.GetAllTagsQuery
{
    public record GetAllTagsQuery :IRequest<IEnumerable<Tag>>;
}
