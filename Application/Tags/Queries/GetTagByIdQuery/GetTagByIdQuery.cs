using MediatR;
using TODO.Core.Models;

namespace TODO.Application.Tags.Queries.GetTagByIdQuery
{
    public record GetTagByIdQuery(int Id) : IRequest<Tag>;
}
