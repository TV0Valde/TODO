using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TODO.WebApi.Controllers;
using TODO.Core.Models;
using TODO.Application.Notes.Commands.CreateNote;
using TODO.Application.Notes.Commands.DeleteNote;
using TODO.Application.Notes.Commands.UpdateNote;
using TODO.Application.Notes.Queries.GetNoteByIdQuery;
using TODO.Application.Notes.Queries.GetAllNotesQuery;

namespace TODO.UnitTests.Controllers
{
    public class NotesControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly NotesController _notesControllerMock;

        public NotesControllerTests() {
            _mediatorMock = new Mock<IMediator>();
            _notesControllerMock = new NotesController(_mediatorMock.Object);
        }

        [Fact]
        public async Task GetAllNotes_ReturnsOkResult_WithListOfNotes() {
            //Arrange
            var notes = new List<Note> { new Note { Id = 1, Title = "Test Note", Description = "Test Description" },
                                         new Note { Id = 2, Title = "Test Note_2", Description = "Test Description_2" } };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAllNotesQuery>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync(notes);

            //Act
            var result = await _notesControllerMock.GetAll();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnNotes = Assert.IsType<List<Note>>(okResult.Value);
            Assert.NotEmpty(returnNotes);
        }

        [Fact]
        public async Task GetNoteById_ReturnsOkResult_WithNote() {

            //Arrange
            var note = new Note { Id = 1, Title = "Test", Description = "Test" };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetNoteByIdQuery>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(note);

            //Act
            var result = await _notesControllerMock.GetById(1);

            //Assert
            var OkResult = Assert.IsType<OkObjectResult>(result);
            var returnNote = Assert.IsType<Note>(OkResult.Value);
            Assert.Equal(1, returnNote.Id);
        }

        [Fact]
        public async Task GetNoteById_ReturnsNotFound_WhenNoteNotExists() {

            //Arrange
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetNoteByIdQuery>(), It.IsAny<CancellationToken>()))
                          .ReturnsAsync((Note)null);

            //Act
            var result = await _notesControllerMock.GetById(99);

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }


        [Fact]
        public async Task Create_ReturnsCreatedAtActionResult_WithNote()
        {
            // Arrange
            var command = new CreateNoteCommand("Test", "Test Description", new List<string> { "tag1" });
            var note = new Note { Id = 1, Title = "Test Note", Description = "Description" };

            _mediatorMock.Setup(m => m.Send(It.IsAny<CreateNoteCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(note);

            // Act
            var result = await _notesControllerMock.Create(command);

            // Assert
            var createdResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(201, createdResult.StatusCode);
            var returnNote = Assert.IsType<Note>(createdResult.Value);
            Assert.Equal(1, returnNote.Id);
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_WhenNoteIsEmpty()
        {
            // Arrange
            var command = new CreateNoteCommand("", "", new List<string> { });

            // Act
            var result = await _notesControllerMock.Create(command);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Ошибка при создании записи.", badRequestResult.Value);
        }


        [Fact]
        public async Task DeleteNote_ReturnsNoContentResult_WhenNoteIsDeleted()
        {
            // Arrange
            _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteNoteCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(true);

            // Act
            var result = await _notesControllerMock.Delete(1);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteNote_ReturnsNotFound_WhenNoteNotExists()
        {
            // Arrange
            _mediatorMock.Setup(m => m.Send(It.IsAny<DeleteNoteCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(false);

            // Act
            var result = await _notesControllerMock.Delete(99);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Update_ReturnsOkResult_WhenNoteIsUpdated()
        {
            // Arrange
            var command = new UpdateNoteCommand(1, "Updated Title", "Updated Description", new List<Tag>());
            var note = new Note { Id = 1, Title = "Updated Note", Description = "Updated Description" };

            _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateNoteCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(note);

            // Act
            var result = await _notesControllerMock.Update(1, command);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnNote = Assert.IsType<Note>(okResult.Value);
            Assert.Equal(1, returnNote.Id);
        }

        [Fact]
        public async Task Update_ReturnsNotFound_WhenNoteNotExists()
        {
            // Arrange
            var command = new UpdateNoteCommand(1, "Updated Title", "Updated Description", new List<Tag>());

            _mediatorMock.Setup(m => m.Send(It.IsAny<UpdateNoteCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync((Note)null);

            // Act
            var result = await _notesControllerMock.Update(1, command);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Update_ReturnsBadRequest_WhenIdMismatch()
        {
            // Arrange
            var command = new UpdateNoteCommand(1, "Updated Title", "Updated Description", new List<Tag>());

            // Act
            var result = await _notesControllerMock.Update(2, command);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }



    }
    }

