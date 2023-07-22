using FluentAssertions;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Moq;
using ProductStore.Application.Features.Authentication.Commands.Register;
using ProductStore.Application.Features.Authentication.Common;
using ProductStore.Application.Interfaces.Persistence;
using ProductStore.Application.Interfaces.Services;

namespace ProductStore.ApplicationTests.Authentication.Register;

public class LoginCommandHandlerTests
{

    private readonly Mock<IMapper> _mapperMock;
    private readonly RegisterCommand _command;
    private readonly RegisterCommandHandler _commandHandler;
    private readonly Mock<UserManager<IdentityUser>> _userManagerMock;
    private readonly Mock<IJwtGenerator> _JwtGeneratorMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public LoginCommandHandlerTests()
    {
        _userManagerMock = new();
        _mapperMock = new();
        _JwtGeneratorMock = new();
        _unitOfWorkMock = new();

        _command = new RegisterCommand(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());
        _commandHandler = new RegisterCommandHandler(
            _unitOfWorkMock.Object,
           _userManagerMock.Object,
           _mapperMock.Object,
           _JwtGeneratorMock.Object
           );
    }



    [Fact]
    public async Task Handle_Should_ReturnAuthResult_WhenInputIsValid()
    {
        //Arrange

        IdentityUser user = MappedUser(_command);
        _mapperMock.Setup(x => x.Map<IdentityUser>(_command))
            .Returns(user);
        var identityResult = IdentityResult.Success;
        _userManagerMock.Setup(x => x.CreateAsync(user, _command.Password))
                             .ReturnsAsync(identityResult);



        var token = "token";

        _JwtGeneratorMock.Setup(x => x.GenerateToken(user))
            .Returns(token);

        AuthResult authResult = CreateAuthResult(user);

        _mapperMock.Setup(x => x.Map<AuthResult>(user))
            .Returns(authResult);
        _unitOfWorkMock.Setup(x => x.SaveChangesAsync())
            .Returns(Task.CompletedTask);


        //Act
        var result = await _commandHandler.Handle(_command, default);
        //Assert
        result.IsError.Should().Be(false);
        result.Value.Should().Be(authResult);

        _userManagerMock.Verify(x => x.CreateAsync(user, _command.Password),
                                     Times.Once);
        result.Value.Token.Should().Be(token);
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

    private IdentityUser MappedUser(RegisterCommand _command)
    {
        return new IdentityUser()
        {
            Email = _command.Email,
            UserName = _command.UserName,
            PasswordHash = _command.Password,
        };
    }

}
