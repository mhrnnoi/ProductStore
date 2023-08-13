using ErrorOr;
using FluentAssertions;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Moq;
using ProductStore.Application.Features.Authentication.Commands.Register;
using ProductStore.Application.Features.Authentication.Common;
using ProductStore.Application.Interfaces.Services;
using ProductStore.Domain.Abstractions;

namespace ProductStore.ApplicationTests.Authentication.Register;

public class RegisterCommandHandlerTests
{

    private readonly Mock<IMapper> _mapperMock;
    private readonly RegisterCommand _command;
    private readonly RegisterCommandHandler _commandHandler;
    private readonly Mock<UserManager<IdentityUser>> _userManagerMock;
    private readonly Mock<IUserStore<IdentityUser>> _userStoreMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IJwtGenerator> _JwtGeneratorMock;


    public RegisterCommandHandlerTests()
    {
        _userManagerMock = new();
        _mapperMock = new();
        _unitOfWorkMock = new();
        _JwtGeneratorMock = new();
        _userStoreMock = new();
        _userManagerMock = new(_userStoreMock.Object, null, null, null, null, null, null, null, null);

        _command = new RegisterCommand(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());
        _commandHandler = new RegisterCommandHandler(
            _unitOfWorkMock.Object,
           _userManagerMock.Object,
           _mapperMock.Object,
           _JwtGeneratorMock.Object
           );
    }



    [Fact]
    public async Task Handle_Should_CreateUserAndReturnAuth_WhenInputIsValid()
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

        _unitOfWorkMock.Setup(x => x.SaveChangesAsync())
            .Returns(Task.CompletedTask);



        AuthResult authResult = CreateAuthResult(user);
        authResult = authResult with { Token = token };

        _mapperMock.Setup(x => x.Map<AuthResult>(_command))
                    .Returns(authResult);


        //Act
        var result = await _commandHandler.Handle(_command, default);
        //Assert
        result.IsError.Should().Be(false);
        result.Value.Should().Be(authResult);
        result.Value.Token.Should().Be(token);


        _userManagerMock.Verify(x => x.CreateAsync(user, _command.Password),
                                     Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        _JwtGeneratorMock.Verify(x => x.GenerateToken(user), Times.Once);
        _userManagerMock.Verify(x => x.CreateAsync(user, _command.Password), Times.Once);



    }
    [Fact]
    public async Task Handle_Should_ReturnBadRequest_WhenUserDosentCreated()
    {
        //Arrange

        IdentityUser user = MappedUser(_command);
        _mapperMock.Setup(x => x.Map<IdentityUser>(_command))
            .Returns(user);
        var identityError = new IdentityError();
        var identityResult = IdentityResult.Failed(identityError);
        var firstError = identityResult.Errors.First();
        _userManagerMock.Setup(x => x.CreateAsync(user, _command.Password))
                             .ReturnsAsync(identityResult);


        //Act
        var result = await _commandHandler.Handle(_command, default);
        //Assert
        result.IsError.Should().Be(true);
        result.FirstError.Should().Be(Error.Failure(firstError.Code, firstError.Description));

        _userManagerMock.Verify(x => x.CreateAsync(user, _command.Password),
                                     Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Never);
        _userManagerMock.Verify(x => x.CreateAsync(user, _command.Password), Times.Once);



    }


    private IdentityUser MappedUser(RegisterCommand _command)
    {
        return new IdentityUser()
        {
            Email = _command.Email,
            UserName = _command.UserName
        };
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
