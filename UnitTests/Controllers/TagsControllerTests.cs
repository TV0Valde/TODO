using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TODO.WebApi.Controllers;
using TODO.Core.Models;
using TODO.Application.Notes.Commands.SetNoteTagsCommand;
using TODO.Application.Reminders.Commands.SetReminderTagsCommand;
using TODO.Application.Reminders.Commands.CreateReminder;
using TODO.Application.Tags.Commands.CreateTagCommand;
using TODO.Application.Tags.Commands.DeleteTagCommand;
using TODO.Application.Tags.Commands.UpdateTagCommand;
using TODO.Application.Tags.Queries.GetAllTagsQuery;
using TODO.Application.Tags.Queries.GetTagByIdQuery;

namespace todo_list.UnitTests.Controllers
{
    public class TagsControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly TagsController _tagsControllerMock;

        public TagsControllerTests() { 
            _mediatorMock = new Mock<IMediator>();
            _tagsControllerMock = new TagsController(_mediatorMock.Object);
        }

        [Fact]
        public async Task GetAllTags_ReturnsOkResult_WithListOfTags() {
            //Arrange
            var tags = new List<Tag> { new Tag { Id = 1, Name = "Test" },
               new Tag { Id =2 , Name = "Test_2" } };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllTagsQuery>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(tags);

            //Act 
            var result = await _tagsControllerMock.GetAll();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnTags = Assert.IsType<List<Tag>>(okResult.Value);
            Assert.NotEmpty(returnTags);
        }

        [Fact]
        public async Task GetTagById_ReturnsOkResult_WithTag()
        {

            //Arrange
            var tag = new Tag { Id = 1, Name = "Test" };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetTagByIdQuery>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(tag);

            //Act
            var result = await _tagsControllerMock.GetById(1);

            //Assert
            var OkResult = Assert.IsType<OkObjectResult>(result);
            var returnTag = Assert.IsType<Tag>(OkResult.Value);
            Assert.Equal(1, returnTag.Id);
        }

        [Fact]
        public async Task GetTagById_ReturnsNotFound_WhenTagNotExists()
        {

            //Arrange
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetTagByIdQuery>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync((Tag)null);

            //Act
            var result = await _tagsControllerMock.GetById(99);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtActionResult_WithTag()
        {
            // Arrange
            var command = new CreateTagCommand("Test");
            var tag = new Tag { Id = 1, Name = "Test Tag" };

            _mediatorMock.Setup(m => m.Send(It.IsAny<CreateTagCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(tag);

            // Act
            var result = await _tagsControllerMock.Create(command);

            // Assert
            var createdResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(201, createdResult.StatusCode);
            var returnTag = Assert.IsType<Tag>(createdResult.Value);
            Assert.Equal(1, returnTag.Id);
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_WhenNoteIsEmpty()
        {
            // Arrange
            var command = new CreateTagCommand( "");

            // Act
            var result = await _tagsControllerMock.Create(command);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Ошибка при создании тэга.", badRequestResult.Value);
        }


        [Fact]
        public async Task DeleteTag_ReturnsNoContentResult_WhenTagIsDeleted()
        {
            // Arrange
            _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteTagCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            // Act
            var result = await _tagsControllerMock.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteTag_ReturnsNotFound_WhenTagNotExists()
        {
            // Arrange
            _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteTagCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(false);

            // Act
            var result = await _tagsControllerMock.Delete(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Update_ReturnsOkResult_WhenTagIsUpdated()
        {
            // Arrange
            var command = new UpdateTagCommand(1, "Updated Name");
            var tag = new Tag { Id = 1, Name = "Updated Tag"};

            _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateTagCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(tag);

            // Act
            var result = await _tagsControllerMock.Update(1, command);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnTag = Assert.IsType<Tag>(okResult.Value);
            Assert.Equal(1, returnTag.Id);
        }

        [Fact]
        public async Task Update_ReturnsNotFound_WhenNoteNotExists()
        {
            // Arrange
            var command = new UpdateTagCommand(1, "Updated Name");

            _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateTagCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync((Tag)null);

            // Act
            var result = await _tagsControllerMock.Update(1, command);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Update_ReturnsBadRequest_WhenIdMismatch()
        {
            // Arrange
            var command = new UpdateTagCommand(1, "Updated Name");

            // Act
            var result = await _tagsControllerMock.Update(2, command);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task setTagsForNote_ReturnsNoContent()
        {
            // Arrange
            int noteId = 1;
            var tagIds = new List<int> { 1, 2, 3 };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<SetNoteTagsCommand>(), default))
                .Returns(Task.FromResult(Unit.Value));

            // Act
            var result = await _tagsControllerMock.SetTagsForNote(noteId, tagIds);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mediatorMock.Verify(m => m.Send(It.Is<SetNoteTagsCommand>(cmd => cmd.NoteId == noteId && cmd.TagIds == tagIds), default), Times.Once);
        }

        [Fact]
        public async Task setTagsForReminders_ReturnsNoContent()
        {
            // Arrange
            int reminderId = 1;
            var tagIds = new List<int> { 1, 2, 3 };

            _mediatorMock
                .Setup(m => m.Send(It.IsAny<SetReminderTagsCommand>(), default))
                .Returns(Task.FromResult(Unit.Value));

            // Act
            var result = await _tagsControllerMock.SetTagsForReminders(reminderId, tagIds);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _mediatorMock.Verify(m => m.Send(It.Is<SetReminderTagsCommand>(cmd => cmd.ReminderId == reminderId && cmd.TagIds == tagIds), default), Times.Once);
        }
    }
}
