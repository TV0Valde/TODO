using MediatR;
using TODO.Core.Models;
namespace TODO.Application.Tags.Commands.UpdateTagCommand
{
    public record UpdateTagCommand(int Id, string Name) :IRequest<Tag>;
}
