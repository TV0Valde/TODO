using Xunit;
using Moq;
using FluentValidation;
using FluentValidation.Results;
using System.Threading;
using System.Threading.Tasks;
using TODO.Core.Models;
using TODO.Application.Tags.Commands.CreateTagCommand;
using TODO.Application.Common.Exceptions;
using TODO.Persistence.Interfaces;

public class CreateTagCommandHandlerTests
{
  
    private readonly Mock<ITagRepository> _tagRepositoryMock;
    private readonly Mock<IValidator<CreateTagCommand>> _validatorMock;
    private readonly CreateTagCommandHandler _handler;

    public CreateTagCommandHandlerTests()
    {
       
        _tagRepositoryMock = new Mock<ITagRepository>();
        _validatorMock = new Mock<IValidator<CreateTagCommand>>();
        _handler = new CreateTagCommandHandler(_tagRepositoryMock.Object,_validatorMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_CreateTag()
    {
        // Arrange
        var command = new CreateTagCommand("Test");

        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<CreateTagCommand>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(new ValidationResult());

        _tagRepositoryMock.Setup(repo => repo.NameExist(It.IsAny<string>()))
                           .Returns(false);


        _tagRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<Tag>()))
                           .ReturnsAsync(new Tag { Id = 1, Name = command.Name});

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(command.Name, result.Name);
        _tagRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<Tag>()), Times.Once);
    }
    [Fact]
    public async Task Handle_DuplicateName_ThrowsDuplecateTitleException()
    {
        // Arrange
        var command = new CreateTagCommand("Duplicate Name");

        _validatorMock.Setup(v => v.ValidateAsync(It.IsAny<CreateTagCommand>(), It.IsAny<CancellationToken>()))
                      .ReturnsAsync(new ValidationResult());

        _tagRepositoryMock.Setup(repo => repo.NameExist(It.IsAny<string>()))
                           .Returns(true);

        // Act & Assert
        await Assert.ThrowsAsync<DuplecateTitleException>(() => _handler.Handle(command, CancellationToken.None));
    }
}