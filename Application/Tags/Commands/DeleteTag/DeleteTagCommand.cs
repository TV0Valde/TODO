using MediatR;
using TODO.Core.Models;
namespace TODO.Application.Tags.Commands.DeleteTagCommand
{
    public record DeleteTagCommand(int Id) :IRequest<bool>;
    
}
