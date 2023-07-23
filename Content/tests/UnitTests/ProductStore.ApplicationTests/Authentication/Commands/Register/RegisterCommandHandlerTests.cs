using ErrorOr;
using FluentAssertions;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Moq;
using ProductStore.Application.Features.Authentication.Commands.Register;
using ProductStore.Application.Interfaces.Persistence;

namespace ProductStore.ApplicationTests.Authentication.Register;

public class LoginCommandHandlerTests
{

    private readonly Mock<IMapper> _mapperMock;
    private readonly RegisterCommand _command;
    private readonly RegisterCommandHandler _commandHandler;
    private readonly Mock<UserManager<IdentityUser>> _userManagerMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    public LoginCommandHandlerTests()
    {
        _userManagerMock = new();
        _mapperMock = new();
        _unitOfWorkMock = new();

        _command = new RegisterCommand(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>());
        _commandHandler = new RegisterCommandHandler(
            _unitOfWorkMock.Object,
           _userManagerMock.Object,
           _mapperMock.Object
           );
    }



    [Fact]
    public async Task Handle_Should_CreateUserAndReturnTrue_WhenInputIsValid()
    {
        //Arrange

        IdentityUser user = MappedUser(_command);
        _mapperMock.Setup(x => x.Map<IdentityUser>(_command))
            .Returns(user);
        var identityResult = IdentityResult.Success;
        _userManagerMock.Setup(x => x.CreateAsync(user, _command.Password))
                             .ReturnsAsync(identityResult);

        _unitOfWorkMock.Setup(x => x.SaveChangesAsync())
            .Returns(Task.CompletedTask);


        //Act
        var result = await _commandHandler.Handle(_command, default);
        //Assert
        result.IsError.Should().Be(false);
        result.Value.Should().Be(true);

        _userManagerMock.Verify(x => x.CreateAsync(user, _command.Password),
                                     Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        _userManagerMock.Verify(x => x.CreateAsync(user, _command.Password), Times.Once);



    }
    [Fact]
    public async Task Handle_Should_ReturnFailure_WhenUserDosentCreated()
    {
        //Arrange

        IdentityUser user = MappedUser(_command);
        _mapperMock.Setup(x => x.Map<IdentityUser>(_command))
            .Returns(user);
        var identityResult = IdentityResult.Failed(new[] { It.IsAny<IdentityError>() });
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

}
