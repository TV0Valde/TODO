using Xunit;
using Moq;
using FluentValidation;
using FluentValidation.Results;
using System.Threading;
using System.Threading.Tasks;
using TODO.Core.Models;
using TODO.Application.Reminders.Commands.CreateReminder;
using TODO.Application.Common.Exceptions;
using TODO.Persistence.Interfaces;

namespace todo_list.UnitTests.Reminders.Commands
{
    public class CreateReminderCommandHandlerTests
    {
        private readonly Mock<IReminderRepository> _reminderRepositoryMock;
        private readonly Mock<ITagRepository> _tagRepositoryMock;
        private readonly Mock<IValidator<CreateReminderCommand>> _validatorMock;
        private readonly CreateReminderCommandHandler _handler;

        public CreateReminderCommandHandlerTests() { 
            _reminderRepositoryMock = new Mock<IReminderRepository>();
            _tagRepositoryMock = new Mock<ITagRepository>();
            _validatorMock = new Mock<IValidator<CreateReminderCommand>>();
            _handler = new CreateReminderCommandHandler(_reminderRepositoryMock.Object, _tagRepositoryMock.Object, _validatorMock.Object);
        }
        [Fact]

        public async Task Handle_ValidCommand_CreateReminder() {
            // Arrange
            var command = new CreateReminderCommand("Test", "Test", DateTime.Now, new List<string> { "Tag1" });

            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<CreateReminderCommand>(), It.IsAny<CancellationToken>()))
                           .ReturnsAsync(new ValidationResult());

            _reminderRepositoryMock.Setup(repo => repo.TitleExist(It.IsAny<string>()))
                                    .Returns(false);

            _reminderRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<Reminder>()))
                                    .ReturnsAsync(new Reminder { Id = 1, Title = command.Title, Description = command.Description, Reminder_time = command.Reminder_time });

            //Act
            var result = await _handler.Handle(command, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(command.Title, result.Title);
            Assert.Equal(command.Description, result.Description);
            Assert.Equal(command.Reminder_time, result.Reminder_time);
            _reminderRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<Reminder>()), Times.Once);

        }
        [Fact]

        public async Task Handle_DuplicateTitle_ThrowsDuplecateTitleException() {
            var command = new CreateReminderCommand("Duplicate Title", "Test Description", DateTime.Now, new List<string> { "tag1" });

            _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<CreateReminderCommand>(), It.IsAny<CancellationToken>()))
                            .ReturnsAsync(new ValidationResult());

            _reminderRepositoryMock.Setup(repo => repo.TitleExist(It.IsAny<string>()))
                                    .Returns(true);

            // Act & Assert
            await Assert.ThrowsAsync<DuplecateTitleException>(() => _handler.Handle(command, CancellationToken.None));
        }
    }
}
