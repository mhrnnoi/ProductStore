using ErrorOr;
using FluentAssertions;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Moq;
using ProductStore.Application.Features.Authentication.Commands.DeleteAccount;
// using ProductStore.Application.Features.Authentication.Commands.DeleteAccount;
using ProductStore.Application.Features.Authentication.Common;
using ProductStore.Application.Interfaces.Persistence;
using ProductStore.Application.Interfaces.Services;

namespace ProductStore.ApplicationTests.Authentication.DeleteAccount;

public class DeleteAccountCommandHandlerTests
{

    private readonly Mock<IMapper> _mapperMock;
    private readonly DeleteAccountCommand _command;
    private readonly DeleteAccountCommandHandler _commandHandler;
    private readonly Mock<UserManager<IdentityUser>> _userManagerMock;
    private readonly Mock<IJwtGenerator> _JwtGeneratorMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;
    private readonly Mock<ICacheService> _cacheServiceMock;


    public DeleteAccountCommandHandlerTests()
    {
        _userManagerMock = new();
        _mapperMock = new();
        _JwtGeneratorMock = new();
        _unitOfWorkMock = new();
        _dateTimeProviderMock = new();
        _cacheServiceMock = new();

        _command = new DeleteAccountCommand(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());
        _commandHandler = new DeleteAccountCommandHandler(
            _unitOfWorkMock.Object,
           _userManagerMock.Object,
           _mapperMock.Object,
           _JwtGeneratorMock.Object,
           _dateTimeProviderMock.Object,
           _cacheServiceMock.Object

           );
    }

    [Fact]
    public async Task Handle_Should_ReturnBadCredential_WhenEmailInvalidAsync()
    {
        //Assert
        _userManagerMock.Setup(x => x.FindByEmailAsync(_command.Email))
                            .ReturnsAsync((IdentityUser?)null);
        //Act
        var result = await _commandHandler.Handle(_command, default);
        //Assert
        result.IsError.Should().Be(true);
        result.FirstError.Should().Be(Error.Failure());
    }

    [Fact]
    public async Task Handle_Should_ReturnBadCredential_WhenPasswordInvalidAsync()
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
        result.FirstError.Should().Be(Error.Failure());
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
        
        var expiryTime = It.IsAny<DateTime>();
        _dateTimeProviderMock.Setup(x => x.UtcNow.AddMinutes(It.IsAny<int>()))
                                        .Returns(expiryTime);
        _cacheServiceMock.Setup(x => x.AddBlacklist(_command.Token, expiryTime))
                            .Returns(true);

        _unitOfWorkMock.Setup(x => x.SaveChangesAsync())
                            .Returns(Task.CompletedTask);

        //Act
        var result = await _commandHandler.Handle(_command, default);
        //Assert

        result.IsError.Should().Be(false);
        result.Value.Should().Be(true);

        _userManagerMock.Verify(x => x.FindByEmailAsync(_command.Email),
                                     Times.Once);

    }

}
