using MediatR;
using Moq;
using TODO.Persistence.Interfaces;
using TODO.Application.Reminders.Commands.SetReminderTagsCommand;

public class SetReminderTagsCommandHandlerTests
{
    private readonly Mock<IReminderRepository> _reminderRepositoryMock;
    private readonly SetReminderTagsCommandHandler _handler;

    public SetReminderTagsCommandHandlerTests()
    {
        _reminderRepositoryMock = new Mock<IReminderRepository>();
        _handler = new SetReminderTagsCommandHandler(_reminderRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Success()
    {
        // Arrange
        int reminderId = 1;
        var tagIds = new List<int> { 1, 2, 3 };
        var command = new SetReminderTagsCommand(reminderId, tagIds);

        _reminderRepositoryMock
            .Setup(r => r.SetTagsAsync(reminderId, tagIds))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal(Unit.Value, result);
        _reminderRepositoryMock.Verify(r => r.SetTagsAsync(reminderId, tagIds), Times.Once);
    }
}