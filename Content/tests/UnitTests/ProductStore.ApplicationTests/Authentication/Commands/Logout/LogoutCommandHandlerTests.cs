using FluentAssertions;
using MapsterMapper;
using Moq;
using ProductStore.Application.Features.Authentication.Commands.Logout;
using ProductStore.Application.Interfaces.Persistence;
using ProductStore.Application.Interfaces.Services;

namespace ProductStore.ApplicationTests.Authentication.Logout;

public class LogoutCommandHandlerTests
{

    private readonly LogoutCommand _command;
    private readonly LogoutCommandHandler _commandHandler;
    private readonly Mock<ICacheService> _cacheServiceMock;
    private readonly Mock<IDateTimeProvider> _dateTimeProviderMock;
    public LogoutCommandHandlerTests()
    {
        _dateTimeProviderMock = new();
        _cacheServiceMock = new();

        _command = new LogoutCommand(It.IsAny<string>());
        _commandHandler = new LogoutCommandHandler(
           _cacheServiceMock.Object,
           _dateTimeProviderMock.Object
           );
    }

    [Fact]
    public async Task Handle_Should_ReturnTrueAndAddTokenToBlacklist()
    {

        //Arrange
        var dateTime =It.IsAny<DateTime>();
        _dateTimeProviderMock.Setup(x => x.UtcNow.AddMinutes(It.IsAny<int>()))
                .Returns(dateTime);
        _cacheServiceMock.Setup(x => x.AddBlacklist(_command.Token, dateTime))
            .Returns(true);
        //Act
        var result =  await _commandHandler.Handle(_command, default);

        //Assert
        result.Should().Be(true);
        result.IsError.Should().Be(false);

    }
}