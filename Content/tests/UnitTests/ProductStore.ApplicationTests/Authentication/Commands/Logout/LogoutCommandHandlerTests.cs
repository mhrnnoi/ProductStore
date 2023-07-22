using MapsterMapper;
using Moq;

namespace ProductStore.ApplicationTests.Authentication.Logout;

public class LogoutCommandHandlerTests
{

    private readonly Mock<IMapper> _mapperMock;
    private readonly LogoutCommand _command;
    private readonly LogoutCommandHandler _commandHandler;

    public LogoutCommandHandlerTests()
    {
        _mapperMock = new();

        _command = new LogoutCommand();
        _commandHandler = new LogoutCommandHandler(
           _mapperMock.Object
           );
    }

    [Fact]
    public async Task Handle_Should_ReturnTrue()
    {

        //Arrange

        //Act

        
        //Assert
       
    }
}