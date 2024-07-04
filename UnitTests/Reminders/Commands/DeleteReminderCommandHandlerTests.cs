using Xunit;
using Moq;
using FluentValidation;
using FluentValidation.Results;
using System.Threading;
using System.Threading.Tasks;
using TODO.Core.Models;
using TODO.Application.Reminders.Commands.DeleteReminder;
using TODO.Application.Common.Exceptions;
using TODO.Persistence.Interfaces;

public class DeleteReminderCommandHandlerTests
{
    private readonly Mock<IReminderRepository> _reminderRepositoryMock;
    private readonly Mock<IValidator<DeleteReminderCommand>> _validatorMock;
    private readonly DeleteReminderCommandHandler _handler;

    public DeleteReminderCommandHandlerTests()
    {
        _reminderRepositoryMock = new Mock<IReminderRepository>();
        _validatorMock = new Mock<IValidator<DeleteReminderCommand>>();
        _handler = new DeleteReminderCommandHandler(_reminderRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_DeletesReminder()
    {
        // Arrange
        var command = new DeleteReminderCommand(1); // Используем корректный конструктор

        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<DeleteReminderCommand>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(new ValidationResult());

        _reminderRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                           .ReturnsAsync(new Reminder { Id = 1, Title = "Title", Description = "Description" });

        _reminderRepositoryMock.Setup(repo => repo.Delete(It.IsAny<int>()));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result);
        _reminderRepositoryMock.Verify(repo => repo.Delete(It.Is<int>(id => id == command.Id)), Times.Once);
    }

    [Fact]
    public async Task Handle_ReminderNotFound_ReturnsFalse()
    {
        // Arrange
        var command = new DeleteReminderCommand(99); // Используем корректный конструктор

        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<DeleteReminderCommand>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(new ValidationResult());

        _reminderRepositoryMock.Setup(repo => repo.GetByIdAsync(It.IsAny<int>()))
                           .ReturnsAsync((Reminder)null);
        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result);
    }
}