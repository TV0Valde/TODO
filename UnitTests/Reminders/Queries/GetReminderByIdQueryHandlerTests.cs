
using Moq;
using TODO.Persistence.Interfaces;
using TODO.Core.Models;
using TODO.Application.Common.Exceptions;
using TODO.Application.Reminders.Queries.GetReminderByIdQuery;

public class GetReminderByIdQueryHandlerTests
{
    private readonly Mock<IReminderRepository> _reminderRepositoryMock;
    private readonly GetReminderByIdQueryHandler _handler;

    public GetReminderByIdQueryHandlerTests()
    {
        _reminderRepositoryMock = new Mock<IReminderRepository>();
        _handler = new GetReminderByIdQueryHandler(_reminderRepositoryMock.Object);
    }
    [Fact]
    public async Task Handle_ReminderExists_ReturnsReminder()
    {
        // Arrange
        var reminder = new Reminder { Id = 1, Title = "Note 1", Description = "Description 1",Reminder_time =DateTime.Now };
        _reminderRepositoryMock.Setup(repo => repo.GetByIdAsync(reminder.Id)).ReturnsAsync(reminder);

        var query = new GetReminderByIdQuery(reminder.Id);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(reminder, result);
    }

    [Fact]
    public async Task Handle_ReminderDoesNotExist_ThrowsNotFoundException()
    {
        // Arrange
        var query = new GetReminderByIdQuery(1);
        _reminderRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Reminder)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(query, CancellationToken.None));
    }
}