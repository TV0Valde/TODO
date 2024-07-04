using Xunit;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TODO.Core.Models;
using TODO.Persistence.Interfaces;
using MediatR;
using TODO.Application.Notes.Queries.GetAllNotesQuery;

public class GetAllNotesQueryHandlerTests
{
    private readonly Mock<INoteRepository> _noteRepositoryMock;
    private readonly Mock<ITagRepository> _tagRepositoryMock;
    private readonly GetAllNotesQueryHandler _handler;

    public GetAllNotesQueryHandlerTests()
    {
        _noteRepositoryMock = new Mock<INoteRepository>();
        _tagRepositoryMock = new Mock<ITagRepository>();
        _handler = new GetAllNotesQueryHandler(_noteRepositoryMock.Object,_tagRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ReturnsListOfNotes()
    {
        // Arrange
        var notes = new List<Note>
        {
            new Note { Id = 1, Title = "Note 1", Description = "Description 1" },
            new Note { Id = 2, Title = "Note 2", Description = "Description 2" }
        };

        _noteRepositoryMock.Setup(repo => repo.GetAllAsync())
                           .ReturnsAsync(notes);

        var query = new GetAllNotesQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(notes, result);
    }

    [Fact]
    public async Task Handle_ReturnsEmptyList_WhenNoNotesExist()
    {
        // Arrange
        _noteRepositoryMock.Setup(repo => repo.GetAllAsync())
                           .ReturnsAsync(new List<Note>());

        var query = new GetAllNotesQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Empty(result);
    }
}
