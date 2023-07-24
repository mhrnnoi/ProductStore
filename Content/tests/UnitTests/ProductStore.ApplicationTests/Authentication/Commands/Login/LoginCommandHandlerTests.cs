using ErrorOr;
using FluentAssertions;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Moq;
using ProductStore.Application.Features.Authentication.Commands.Login;
using ProductStore.Application.Features.Authentication.Common;
using ProductStore.Application.Interfaces.Persistence;
using ProductStore.Application.Interfaces.Services;

namespace ProductStore.ApplicationTests.Authentication.Login;

public class LoginCommandHandlerTests
{

    private readonly Mock<IMapper> _mapperMock;
    private readonly LoginCommand _command;
    private readonly LoginCommandHandler _commandHandler;
    private readonly Mock<UserManager<IdentityUser>> _userManagerMock;
    private readonly Mock<IUserStore<IdentityUser>> _userStoreMock;

    private readonly Mock<IJwtGenerator> _JwtGeneratorMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public LoginCommandHandlerTests()
    {
        _userManagerMock = new();
        _mapperMock = new();
        _JwtGeneratorMock = new();
        _unitOfWorkMock = new();
        _userStoreMock = new();

        _userManagerMock = new(_userStoreMock.Object, null, null, null, null, null, null, null, null);

        _command = new LoginCommand(It.IsAny<string>(), It.IsAny<string>());
        _commandHandler = new LoginCommandHandler(
            _unitOfWorkMock.Object,
           _userManagerMock.Object,
           _mapperMock.Object,
           _JwtGeneratorMock.Object
           );
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenEmailInvalidAsync()
    {
        //Arrange
        _userManagerMock.Setup(x => x.FindByEmailAsync(_command.Email))
                            .ReturnsAsync((IdentityUser?)null);
        //Act
        var result = await _commandHandler.Handle(_command, default);
        //Assert
        result.IsError.Should().Be(true);
        result.FirstError.Should().Be(Error.Failure(description: "Bad Credential"));
    }

    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenPasswordInvalidAsync()
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
    public async Task Handle_Should_ReturnAuthResult_WhenInputIsValid()
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
        result.Value.Token.Should().Be(token);

        _userManagerMock.Verify(x => x.FindByEmailAsync(_command.Email),
                                     Times.Once);
        _userManagerMock.Verify(x => x.CheckPasswordAsync(user, _command.Password),
                                     Times.Once);
        _JwtGeneratorMock.Verify(x => x.GenerateToken(user), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);



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
