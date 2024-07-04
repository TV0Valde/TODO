using Xunit;
using Moq;
using FluentValidation;
using FluentValidation.Results;
using System.Threading;
using System.Threading.Tasks;
using TODO.Core.Models;
using TODO.Application.Tags.Commands.UpdateTagCommand;
using TODO.Application.Common.Exceptions;
using TODO.Persistence.Interfaces;

public class UpdateTagCommandHandlerTests
{
    private readonly Mock<ITagRepository> _tagRepositoryMock;
    private readonly Mock<IValidator<UpdateTagCommand>> _validatorMock;
    private readonly UpdateTagCommandHandler _handler;

    public UpdateTagCommandHandlerTests()
    {
        _tagRepositoryMock = new Mock<ITagRepository>();
        _validatorMock = new Mock<IValidator<UpdateTagCommand>>();
        _handler = new UpdateTagCommandHandler(_tagRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_UpdatesTag()
    {
        // Arrange
        var command = new UpdateTagCommand(1, "Updated Title");

        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<UpdateTagCommand>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(new ValidationResult());

        _tagRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                           .ReturnsAsync(new Tag { Id = 1, Name = "Old Title"});

        _tagRepositoryMock.Setup(repo => repo.Update(It.IsAny<Tag>()));

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _tagRepositoryMock.Verify(repo => repo.Update(It.Is<Tag>(n => n.Name == command.Name)), Times.Once);
    }

    [Fact]
    public async Task Handle_TagNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var command = new UpdateTagCommand(99, "Updated Title");

        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<UpdateTagCommand>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(new ValidationResult());

        _tagRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                           .ReturnsAsync((Tag)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }
}
