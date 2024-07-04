using Xunit;
using Moq;
using FluentValidation;
using FluentValidation.Results;
using System.Threading;
using System.Threading.Tasks;
using TODO.Core.Models;
using TODO.Application.Notes.Commands.DeleteNote;
using TODO.Application.Common.Exceptions;
using TODO.Persistence.Interfaces;

public class DeleteNoteCommandHandlerTests
{
    private readonly Mock<INoteRepository> _noteRepositoryMock;
    private readonly Mock<IValidator<DeleteNoteCommand>> _validatorMock;
    private readonly DeleteNoteCommandHandler _handler;

    public DeleteNoteCommandHandlerTests()
    {
        _noteRepositoryMock = new Mock<INoteRepository>();
        _validatorMock = new Mock<IValidator<DeleteNoteCommand>>();
        _handler = new DeleteNoteCommandHandler(_noteRepositoryMock.Object, _validatorMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_DeletesNote()
    {
        // Arrange
        var command = new DeleteNoteCommand(1); // Используем корректный конструктор

        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<DeleteNoteCommand>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(new ValidationResult());

        _noteRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                           .ReturnsAsync(new Note { Id = 1, Title = "Title", Description = "Description" });

        _noteRepositoryMock.Setup(repo => repo.Delete(It.IsAny<int>())); 

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
        _noteRepositoryMock.Verify(repo => repo.Delete(It.Is<int>(id => id == command.Id)), Times.Once);
    }

    [Fact]
    public async Task Handle_NoteNotFound_ReturnsFalse()
    {
        // Arrange
        var command = new DeleteNoteCommand(99); // Используем корректный конструктор

        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<DeleteNoteCommand>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(new ValidationResult());

        _noteRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                           .ReturnsAsync((Note)null);
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result);
    }
}