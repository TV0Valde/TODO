using Xunit;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TODO.Core.Models;
using TODO.Persistence.Interfaces;
using MediatR;
using TODO.Application.Reminders.Queries.GetAllRemindersQuery;

public class GetAllRemindersQueryHandlerTests
{
    private readonly Mock<IReminderRepository> _reminderRepositoryMock;
    private readonly Mock<ITagRepository> _tagRepositoryMock;
    private readonly GetAllRemindersQueryHandler _handler;

    public GetAllRemindersQueryHandlerTests()
    {
        _reminderRepositoryMock = new Mock<IReminderRepository>();
        _tagRepositoryMock = new Mock<ITagRepository>();
        _handler = new GetAllRemindersQueryHandler(_reminderRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ReturnsListOfReminders()
    {
        // Arrange
        var reminders = new List<Reminder>
        {
            new Reminder { Id = 1, Title = "Note 1", Description = "Description 1" , Reminder_time = DateTime.Now},
            new Reminder { Id = 2, Title = "Note 2", Description = "Description 2",Reminder_time = DateTime.Now }
        };

        _reminderRepositoryMock.Setup(repo => repo.GetAllAsync())
                           .ReturnsAsync(reminders);

        var query = new GetAllRemindersQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(reminders, result);
    }

    [Fact]
    public async Task Handle_ReturnsEmptyList_WhenNoNotesExist()
    {
        // Arrange
        _reminderRepositoryMock.Setup(repo => repo.GetAllAsync())
                           .ReturnsAsync(new List<Reminder>());

        var query = new GetAllRemindersQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Empty(result);
    }
}
