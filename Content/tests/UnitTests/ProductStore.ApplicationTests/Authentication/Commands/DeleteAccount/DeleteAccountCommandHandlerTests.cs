using ErrorOr;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Moq;
using ProductStore.Application.Features.Authentication.Commands.DeleteAccount;
using ProductStore.Domain.Abstractions;

namespace ProductStore.ApplicationTests.Authentication.DeleteAccount;

public class DeleteAccountCommandHandlerTests
{

    private readonly DeleteAccountCommand _command;
    private readonly DeleteAccountCommandHandler _commandHandler;
    private readonly Mock<UserManager<IdentityUser>> _userManagerMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IUserStore<IdentityUser>> _userStoreMock;
    private readonly Mock<ICacheService> _chacheServiceMock;



    public DeleteAccountCommandHandlerTests()
    {
        _userStoreMock = new();

        _userManagerMock = new(_userStoreMock.Object, null, null, null, null, null, null, null, null);
        _chacheServiceMock = new();
        _unitOfWorkMock = new();

        _command = new DeleteAccountCommand(It.IsAny<string>(), It.IsAny<string>());
        _commandHandler = new DeleteAccountCommandHandler(
            _unitOfWorkMock.Object,
           _userManagerMock.Object,
           _chacheServiceMock.Object

           );
    }

    [Fact]
    public async Task Handle_Should_ReturnBadRequest_WhenEmailInvalidAsync()
    {
        //Assert
        _userManagerMock.Setup(x => x.FindByEmailAsync(_command.Email))
                            .ReturnsAsync((IdentityUser?)null);
        //Act
        var result = await _commandHandler.Handle(_command, default);
        //Assert
        result.IsError.Should().Be(true);
        result.FirstError.Should().Be(Error.Failure(description: "Bad Credential"));
    }

    [Fact]
    public async Task Handle_Should_ReturnBadRequest_WhenPasswordInvalidAsync()
    {
        //Arrange
        var user = new IdentityUser()
        {
            Email = _command.Email,
            PasswordHash = _command.Password
        };
        _userManagerMock.Setup(x => x.FindByEmailAsync(_command.Email))
                            .ReturnsAsync(user);
        _userManagerMock.Setup(x => x.CheckPasswordAsync(user, _command.Password))
                            .ReturnsAsync(false);
        //Act
        var result = await _commandHandler.Handle(_command, default);
        //Assert
        result.IsError.Should().Be(true);
        result.FirstError.Should().Be(Error.Failure(description: "Bad Credential"));

    }


    [Fact]
    public async Task Handle_Should_ReturnTrueAndDeleteAccount_WhenInputIsValid()
    {
        //Arrange
        var user = new IdentityUser()
        {
            Email = _command.Email,
            PasswordHash = _command.Password
        };
        _userManagerMock.Setup(x => x.FindByEmailAsync(_command.Email))
                             .ReturnsAsync(user);
        _userManagerMock.Setup(x => x.CheckPasswordAsync(user, _command.Password))
                            .ReturnsAsync(true);
        _userManagerMock.Setup(x => x.DeleteAsync(user))
                            .ReturnsAsync(IdentityResult.Success);
        _chacheServiceMock.Setup(x => x.RemoveData("products"));




        _unitOfWorkMock.Setup(x => x.SaveChangesAsync())
                            .Returns(Task.CompletedTask);

        //Act
        var result = await _commandHandler.Handle(_command, default);
        //Assert

        result.IsError.Should().Be(false);
        result.Value.Should().Be(true);

        _userManagerMock.Verify(x => x.FindByEmailAsync(_command.Email),
                                     Times.Once);
        _userManagerMock.Verify(x => x.CheckPasswordAsync(user, _command.Password),
                                     Times.Once);
        _userManagerMock.Verify(x => x.DeleteAsync(user),
                                     Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(),
                                     Times.Once);
        _chacheServiceMock.Verify(x => x.RemoveData("products"),
                                     Times.Once);

    }

}
