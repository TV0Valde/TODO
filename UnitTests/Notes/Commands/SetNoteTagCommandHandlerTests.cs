using Xunit;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using TODO.Persistence.Interfaces;
using TODO.Application.Notes.Commands.SetNoteTagsCommand;
using MediatR;

public class SetNoteTagsCommandHandlerTests
{
    private readonly Mock<INoteRepository> _noteRepositoryMock;
    private readonly SetNoteTagsCommandHandler _handler;

    public SetNoteTagsCommandHandlerTests()
    {
        _noteRepositoryMock = new Mock<INoteRepository>();
        _handler = new SetNoteTagsCommandHandler(_noteRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Success()
    {
        // Arrange
        int noteId = 1;
        var tagIds = new List<int> { 1, 2, 3 };
        var command = new SetNoteTagsCommand(noteId, tagIds);

        _noteRepositoryMock
            .Setup(r => r.SetTagsAsync(noteId, tagIds))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(Unit.Value, result);
        _noteRepositoryMock.Verify(r => r.SetTagsAsync(noteId, tagIds), Times.Once);
    }
}