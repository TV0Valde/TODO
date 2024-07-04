using Xunit;
using Moq;
using FluentValidation;
using FluentValidation.Results;
using System.Threading;
using System.Threading.Tasks;
using TODO.Core.Models;
using TODO.Application.Tags.Commands.DeleteTagCommand;
using TODO.Application.Common.Exceptions;
using TODO.Persistence.Interfaces;

public class DeleteTagCommandHandlerTests
{
    private readonly Mock<ITagRepository> _tagRepositoryMock;
    private readonly Mock<IValidator<DeleteTagCommand>> _validatorMock;
    private readonly DeleteTagCommandHandler _handler;

    public DeleteTagCommandHandlerTests()
    {
        _tagRepositoryMock = new Mock<ITagRepository>();
        _validatorMock = new Mock<IValidator<DeleteTagCommand>>();
        _handler = new DeleteTagCommandHandler(_tagRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_DeletesTag()
    {
        // Arrange
        var command = new DeleteTagCommand(1); // Используем корректный конструктор

        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<DeleteTagCommand>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(new ValidationResult());

        _tagRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                           .ReturnsAsync(new Tag { Id = 1, Name = "Name" });

        _tagRepositoryMock.Setup(repo => repo.Delete(It.IsAny<int>()));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
        _tagRepositoryMock.Verify(repo => repo.Delete(It.Is<int>(id => id == command.Id)), Times.Once);
    }

    [Fact]
    public async Task Handle_TagNotFound_ReturnsFalse()
    {
        // Arrange
        var command = new DeleteTagCommand(99); // Используем корректный конструктор

        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<DeleteTagCommand>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(new ValidationResult());

        _tagRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                           .ReturnsAsync((Tag)null);
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result);
    }
}