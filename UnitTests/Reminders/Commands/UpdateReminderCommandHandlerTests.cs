using Moq;
using FluentValidation;
using TODO.Core.Models;
using TODO.Application.Reminders.Commands.UpdateReminder;
using TODO.Application.Common.Exceptions;
using TODO.Persistence.Interfaces;
using FluentValidation.Results;

public class UpdateReminderCommandHandlerTests
{
    private readonly Mock<IReminderRepository> _reminderRepositoryMock;
    private readonly Mock<ITagRepository> _tagRepositoryMock;
    private readonly Mock<IValidator<UpdateReminderCommand>> _validatorMock;
    private readonly UpdateReminderCommandHandler _handler;

    public UpdateReminderCommandHandlerTests()
    {
        _reminderRepositoryMock = new Mock<IReminderRepository>();
        _tagRepositoryMock = new Mock<ITagRepository>();
        _validatorMock = new Mock<IValidator<UpdateReminderCommand>>();
        _handler = new UpdateReminderCommandHandler(_reminderRepositoryMock.Object, _validatorMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_UpdatesReminder()
    {
        // Arrange
        var command = new UpdateReminderCommand(1, "Updated Title", "Updated Description", DateTime.Now);

        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<UpdateReminderCommand>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(new ValidationResult());

        _reminderRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                           .ReturnsAsync(new Reminder { Id = 1, Title = "Old Title", Description = "Old Description", Reminder_time =DateTime.Now });

        _reminderRepositoryMock.Setup(repo => repo.Update(It.IsAny<Reminder>()));

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _reminderRepositoryMock.Verify(repo => repo.Update(It.Is<Reminder>(n => n.Title == command.Title && n.Description == command.Description)), Times.Once);
    }

    [Fact]
    public async Task Handle_ReminderNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var command = new UpdateReminderCommand(99, "Updated Title", "Updated Description", DateTime.Now);

        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<UpdateReminderCommand>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(new ValidationResult());

        _reminderRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                           .ReturnsAsync((Reminder)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(command, CancellationToken.None));
    }
}
