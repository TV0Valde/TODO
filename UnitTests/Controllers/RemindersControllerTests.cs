using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TODO.WebApi.Controllers;
using TODO.Core.Models;
using TODO.Application.Reminders.Commands.CreateReminder;
using TODO.Application.Reminders.Commands.DeleteReminder;
using TODO.Application.Reminders.Commands.UpdateReminder;
using TODO.Application.Reminders.Queries.GetReminderByIdQuery;
using TODO.Application.Reminders.Queries.GetAllRemindersQuery;
using TODO.Application.Notes.Commands.CreateNote;

namespace todo_list.UnitTests.Controllers
{
    public class RemindersControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly RemindersController _remindersControllerMock;

        public RemindersControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _remindersControllerMock = new RemindersController(_mediatorMock.Object);
        }

        [Fact]
        public async Task GetAllReminders_ReturnsOkResult_WithListOfReminders()
        {
            //Arrange
            var reminders = new List<Reminder> { new Reminder { Id = 1, Title = "Test Reminder", Description = "Test Description" },
                                         new Reminder { Id = 2, Title = "Test Reminder_2", Description = "Test Description_2" } };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllRemindersQuery>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(reminders);

            //Act
            var result = await _remindersControllerMock.GetAll();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnReminders = Assert.IsType<List<Reminder>>(okResult.Value);
            Assert.NotEmpty(returnReminders);
        }

        [Fact]
        public async Task GetReminderById_ReturnsOkResult_WithReminder()
        {

            //Arrange
            var reminder = new Reminder { Id = 1, Title = "Test", Description = "Test" };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetReminderByIdQuery>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(reminder);

            //Act
            var result = await _remindersControllerMock.GetById(1);

            //Assert
            var OkResult = Assert.IsType<OkObjectResult>(result);
            var returnReminder = Assert.IsType<Reminder>(OkResult.Value);
            Assert.Equal(1, returnReminder.Id);
        }

        [Fact]
        public async Task GetReminderById_ReturnsNotFound_WhenReminderNotExists()
        {

            //Arrange
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetReminderByIdQuery>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync((Reminder)null);

            //Act
            var result = await _remindersControllerMock.GetById(99);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }


        [Fact]
        public async Task Create_ReturnsCreatedAtActionResult_WithReminder()
        {
            // Arrange
            var command = new CreateReminderCommand("Test", "Test Description", DateTime.Now, new List<string> { "tag1" });
            var reminder = new Reminder { Id = 1, Title = "Test Reminder", Reminder_time= DateTime.Now, Description = "Description" };

            _mediatorMock.Setup(m => m.Send(It.IsAny<CreateReminderCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(reminder);

            // Act
            var result = await _remindersControllerMock.Create(command);

            // Assert
            var createdResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(201, createdResult.StatusCode);
            var returnReminder = Assert.IsType<Reminder>(createdResult.Value);
            Assert.Equal(1, returnReminder.Id);
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_WhenNoteIsEmpty()
        {
            // Arrange
            var command = new CreateReminderCommand("", "", DateTime.MinValue, new List<string> {  });

            // Act
            var result = await _remindersControllerMock.Create(command);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Ошибка при создании напоминания.", badRequestResult.Value);
        }

        [Fact]
        public async Task DeleteReminder_ReturnsNoContentResult_WhenReminderIsDeleted()
        {
            // Arrange
            _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteReminderCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            // Act
            var result = await _remindersControllerMock.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteReminder_ReturnsNotFound_WhenReminderNotExists()
        {
            // Arrange
            _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteReminderCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(false);

            // Act
            var result = await _remindersControllerMock.Delete(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Update_ReturnsOkResult_WhenReminderIsUpdated()
        {
            // Arrange
            var command = new UpdateReminderCommand(1, "Updated Title", "Updated Description", DateTime.Now);
            var reminder = new Reminder { Id = 1, Title = "Updated Reminder", Description = "Updated Description" };

            _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateReminderCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(reminder);

            // Act
            var result = await _remindersControllerMock.Update(1, command);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnReminder = Assert.IsType<Reminder>(okResult.Value);
            Assert.Equal(1, returnReminder.Id);
        }

        [Fact]
        public async Task Update_ReturnsNotFound_WhenReminderNotExists()
        {
            // Arrange
            var command = new UpdateReminderCommand(1, "Updated Title", "Updated Description", DateTime.Now);

            _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateReminderCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync((Reminder)null);

            // Act
            var result = await _remindersControllerMock.Update(1, command);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Update_ReturnsBadRequest_WhenIdMismatch()
        {
            // Arrange
            var command = new UpdateReminderCommand(1, "Updated Title", "Updated Description", DateTime.Now);

            // Act
            var result = await _remindersControllerMock.Update(2, command);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }



    }
}

