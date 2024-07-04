using Xunit;
using Moq;
using FluentValidation;
using FluentValidation.Results;
using System.Threading;
using System.Threading.Tasks;
using TODO.Core.Models;
using TODO.Application.Notes.Commands.UpdateNote;
using TODO.Application.Common.Exceptions;
using TODO.Persistence.Interfaces;

public class UpdateNoteCommandHandlerTests
{
    private readonly Mock<INoteRepository> _noteRepositoryMock;
    private readonly Mock<ITagRepository> _tagRepositoryMock;
    private readonly Mock<IValidator<UpdateNoteCommand>> _validatorMock;
    private readonly UpdateNoteCommandHandler _handler;

    public UpdateNoteCommandHandlerTests()
    {
        _noteRepositoryMock = new Mock<INoteRepository>();
        _tagRepositoryMock = new Mock<ITagRepository>();
        _validatorMock = new Mock<IValidator<UpdateNoteCommand>>();
        _handler = new UpdateNoteCommandHandler(_noteRepositoryMock.Object, _tagRepositoryMock.Object, _validatorMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_UpdatesNote()
    {
        // Arrange
        var command = new UpdateNoteCommand(1, "Updated Title", "Updated Description", new List<Tag>());

        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<UpdateNoteCommand>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(new ValidationResult());

        _noteRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                           .ReturnsAsync(new Note { Id = 1, Title = "Old Title", Description = "Old Description" });

        _noteRepositoryMock.Setup(repo => repo.Update(It.IsAny<Note>()));

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _noteRepositoryMock.Verify(repo => repo.Update(It.Is<Note>(n => n.Title == command.Title && n.Description == command.Description)), Times.Once);
    }

    [Fact]
    public async Task Handle_NoteNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var command = new UpdateNoteCommand(99, "Updated Title", "Updated Description", new List<Tag>());

        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<UpdateNoteCommand>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(new ValidationResult());

        _noteRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                           .ReturnsAsync((Note)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }
}
