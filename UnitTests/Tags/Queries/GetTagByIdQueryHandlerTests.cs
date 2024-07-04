using Xunit;
using Moq;
using TODO.Persistence.Interfaces;
using TODO.Core.Models;
using System.Threading;
using System.Threading.Tasks;
using TODO.Application.Common.Exceptions;
using TODO.Application.Tags.Queries.GetTagByIdQuery;

public class GetTagByIdQueryHandlerTests
{
    private readonly Mock<ITagRepository> _tagRepositoryMock;
    private readonly GetTagByIdQueryHandler _handler;

    public GetTagByIdQueryHandlerTests()
    {
        _tagRepositoryMock = new Mock<ITagRepository>();
        _handler = new GetTagByIdQueryHandler(_tagRepositoryMock.Object);
    }
    [Fact]
    public async Task Handle_TagExists_ReturnsTag()
    {
        // Arrange
        var tag = new Tag { Id = 1, Name = "Tag 1" };
        _tagRepositoryMock.Setup(repo => repo.GetByIdAsync(tag.Id)).ReturnsAsync(tag);

        var query = new GetTagByIdQuery(tag.Id);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(tag, result);
    }

    [Fact]
    public async Task Handle_TagDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var query = new GetTagByIdQuery(1);
        _tagRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Tag)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(query, CancellationToken.None));
    }
}