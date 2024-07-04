using MediatR;
using TODO.Core.Models;

namespace TODO.Application.Tags.Commands.CreateTagCommand
{
    public record CreateTagCommand(string Name) : IRequest<Tag>;
   
}
