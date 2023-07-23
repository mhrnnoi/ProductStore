using ErrorOr;
using FluentAssertions;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Moq;
using ProductStore.Application.Features.Authentication.Commands.ResetPassword;
// using ProductStore.Application.Features.Authentication.Commands.ResetPassword;
using ProductStore.Application.Features.Authentication.Common;
using ProductStore.Application.Interfaces.Persistence;
using ProductStore.Application.Interfaces.Services;

namespace ProductStore.ApplicationTests.Authentication.ResetPassword;

public class ResetPasswordCommandHandlerTests
{

    private readonly Mock<IMapper> _mapperMock;
    private readonly ResetPasswordCommand _command;
    private readonly ResetPasswordCommandHandler _commandHandler;
    private readonly Mock<UserManager<IdentityUser>> _userManagerMock;
    private readonly Mock<IJwtGenerator> _JwtGeneratorMock;
    private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;

    private readonly Mock<ICacheService> _cacheServiceMock;

    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public ResetPasswordCommandHandlerTests()
    {
        _userManagerMock = new();
        _mapperMock = new();
        _JwtGeneratorMock = new();
        _unitOfWorkMock = new();
        _dateTimeProviderMock = new();
        _cacheServiceMock = new();


        _command = new ResetPasswordCommand(It.IsAny<string>(),
                                            It.IsAny<string>(),
                                            It.IsAny<string>(),
                                            It.IsAny<string>());

        _commandHandler = new ResetPasswordCommandHandler(
            _unitOfWorkMock.Object,
           _userManagerMock.Object,
           _mapperMock.Object,
           _JwtGeneratorMock.Object,
           _cacheServiceMock.Object,
           _dateTimeProviderMock.Object

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
            PasswordHash = _command.OldPassword
        };
        _userManagerMock.Setup(x => x.FindByEmailAsync(_command.Email))
                            .ReturnsAsync(user);
        _userManagerMock.Setup(x => x.CheckPasswordAsync(user, _command.OldPassword))
                            .ReturnsAsync(false);

        //Act
        var result = await _commandHandler.Handle(_command, default);
        //Assert
        result.IsError.Should().Be(true);
        result.FirstError.Should().Be(Error.Failure());
    }


    [Fact]
    public async Task Handle_Should_ReturnAuthResult_WhenInputIsValid()
    {
        // Arrange


        var user = new IdentityUser()
        {
            Email = _command.Email,
            PasswordHash = _command.OldPassword
        };
        _userManagerMock.Setup(x => x.FindByEmailAsync(_command.Email))
                             .ReturnsAsync(user);
        _userManagerMock.Setup(x => x.CheckPasswordAsync(user, _command.OldPassword))
                            .ReturnsAsync(true);
        _userManagerMock.Setup(x => x.ChangePasswordAsync(user, _command.OldPassword, _command.NewPassword))
                            .ReturnsAsync(IdentityResult.Success);
        var expiryTime = It.IsAny<DateTime>();
        _dateTimeProviderMock.Setup(x => x.UtcNow.AddMinutes(It.IsAny<int>()))
                                        .Returns(expiryTime);
        _cacheServiceMock.Setup(x => x.AddBlacklist(_command.Token, expiryTime))
                      .Returns(true);

        var token = "token";
        _JwtGeneratorMock.Setup(x => x.GenerateToken(user))
                            .Returns(token);

        _unitOfWorkMock.Setup(x => x.SaveChangesAsync())
                            .Returns(Task.CompletedTask);

        AuthResult authResult = CreateAuthResult(user);
        authResult = authResult with { Token = token };

        _mapperMock.Setup(x => x.Map<AuthResult>(user))
                    .Returns(authResult);


        //Act
        var result = await _commandHandler.Handle(_command, default);
        //Assert

        result.IsError.Should().Be(false);
        result.Value.Should().Be(authResult);

        _userManagerMock.Verify(x => x.FindByEmailAsync(_command.Email),
                                     Times.Once);
        result.Value.Token.Should().Be(token);



    }

    private static AuthResult CreateAuthResult(IdentityUser user)
    {
        return new AuthResult(
                              user.Email,
                              user.UserName,
                              It.IsAny<string>()
                              );
    }
}
