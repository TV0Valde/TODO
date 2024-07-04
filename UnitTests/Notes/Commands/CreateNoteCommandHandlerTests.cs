using Xunit;
using Moq;
using FluentValidation;
using FluentValidation.Results;
using System.Threading;
using System.Threading.Tasks;
using TODO.Core.Models;
using TODO.Application.Notes.Commands.CreateNote;
using TODO.Application.Common.Exceptions;
using TODO.Persistence.Interfaces;
using TODO.Application.Notes.Commands;

public class CreateNoteCommandHandlerTests
{
    private readonly Mock<INoteRepository> _noteRepositoryMock;
    private readonly Mock<ITagRepository> _tagRepositoryMock;
    private readonly Mock<IValidator<CreateNoteCommand>> _validatorMock;
    private readonly CreateNoteCommandHandler _handler;

    public CreateNoteCommandHandlerTests()
    {
        _noteRepositoryMock = new Mock<INoteRepository>();
        _tagRepositoryMock = new Mock<ITagRepository>();
        _validatorMock = new Mock<IValidator<CreateNoteCommand>>();
        _handler = new CreateNoteCommandHandler(_noteRepositoryMock.Object, _tagRepositoryMock.Object, _validatorMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_CreateNote()
    {
        // Arrange
        var command = new CreateNoteCommand("Test", "Test Description", new List<string> { "tag1" });

        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<CreateNoteCommand>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(new ValidationResult());

        _noteRepositoryMock.Setup(repo => repo.TitleExist(It.IsAny<string>()))
                           .Returns(false);


        _noteRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<Note>()))
                           .ReturnsAsync(new Note { Id = 1, Title = command.Title, Description = command.Description });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(command.Title, result.Title);
        Assert.Equal(command.Description, result.Description);
        _noteRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<Note>()), Times.Once);
    }
    [Fact]
    public async Task Handle_DuplicateTitle_ThrowsDuplecateTitleException()
    {
        // Arrange
        var command = new CreateNoteCommand("Duplicate Title", "Test Description", new List<string> { "tag1" });

        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<CreateNoteCommand>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(new ValidationResult());

        _noteRepositoryMock.Setup(repo => repo.TitleExist(It.IsAny<string>()))
                           .Returns(true);

        // Act & Assert
        await Assert.ThrowsAsync<DuplecateTitleException>(() => _handler.Handle(command, CancellationToken.None));
    }
}