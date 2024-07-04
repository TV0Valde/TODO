using Xunit;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TODO.Core.Models;
using TODO.Persistence.Interfaces;
using MediatR;
using TODO.Application.Tags.Queries.GetAllTagsQuery;

public class GetAlltagsQueryHandlerTests
{
    private readonly Mock<ITagRepository> _tagRepositoryMock;
 
    private readonly GetAllTagsQueryHandler _handler;

    public GetAlltagsQueryHandlerTests()
    {
        _tagRepositoryMock = new Mock<ITagRepository>();
       
        _handler = new GetAllTagsQueryHandler( _tagRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ReturnsListOfTags()
    {
        // Arrange
        var tags = new List<Tag>
        {
            new Tag { Id = 1, Name = "tag 1",},
            new Tag { Id = 2, Name = "tag 2"}
        };

        _tagRepositoryMock.Setup(repo => repo.GetAllAsync())
                           .ReturnsAsync(tags);

        var query = new GetAllTagsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(tags, result);
    }

    [Fact]
    public async Task Handle_ReturnsEmptyList_WhenNoTagsExist()
    {
        // Arrange
        _tagRepositoryMock.Setup(repo => repo.GetAllAsync())
                           .ReturnsAsync(new List<Tag>());

        var query = new GetAllTagsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Empty(result);
    }
}
