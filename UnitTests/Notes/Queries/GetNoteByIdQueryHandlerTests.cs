using Xunit;
using Moq;
using TODO.Persistence.Interfaces;
using TODO.Core.Models;
using System.Threading;
using System.Threading.Tasks;
using TODO.Application.Common.Exceptions;
using TODO.Application.Notes.Queries.GetNoteByIdQuery;

public class GetNoteByIdQueryHandlerTests
{
    private readonly Mock<INoteRepository> _noteRepositoryMock;
    private readonly GetNoteByIdQueryHandler _handler;

    public GetNoteByIdQueryHandlerTests()
    {
        _noteRepositoryMock = new Mock<INoteRepository>();
        _handler = new GetNoteByIdQueryHandler(_noteRepositoryMock.Object);
    }
    [Fact]
    public async Task Handle_NoteExists_ReturnsNote()
    {
        // Arrange
        var note = new Note { Id = 1, Title = "Note 1", Description = "Description 1" };
        _noteRepositoryMock.Setup(repo => repo.GetByIdAsync(note.Id)).ReturnsAsync(note);

        var query = new GetNoteByIdQuery(note.Id);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(note, result);
    }

    [Fact]
    public async Task Handle_NoteDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var query = new GetNoteByIdQuery(1);
        _noteRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Note)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(query, CancellationToken.None));
    }
}