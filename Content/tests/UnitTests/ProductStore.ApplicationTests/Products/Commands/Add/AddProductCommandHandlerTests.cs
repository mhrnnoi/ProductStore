using ErrorOr;
using FluentAssertions;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Moq;
using ProductStore.Application.Features.Products.Commands.Add;
using ProductStore.Application.Interfaces.Persistence;
using ProductStore.Domain.Products.Entities;

namespace ProductStore.ApplicationTests.Products.Commands.Add;

public class AddProductCommandHandlerTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly AddProductCommand _command;
    private readonly AddProductCommandHandler _commandHandler;
    private readonly Mock<UserManager<IdentityUser>> _userManagerMock;
    private readonly Mock<IUserStore<IdentityUser>> _userStoreMock;
        private readonly Mock<ICacheService> _chacheServiceMock;


    public AddProductCommandHandlerTests()
    {
        _productRepositoryMock = new();
        _mapperMock = new();
        _unitOfWorkMock = new();
        _userStoreMock = new();
        _chacheServiceMock = new();

        _userManagerMock = new(_userStoreMock.Object, null, null, null, null, null, null, null, null);


        _command = new AddProductCommand(It.IsAny<string>(),
                                         It.IsAny<bool>(),
                                         It.IsAny<string>(),
                                         It.IsAny<string>(),
                                         It.IsAny<DateTime>(),
                                         It.IsAny<string>());

        _commandHandler = new AddProductCommandHandler(_unitOfWorkMock.Object,
                                                       _productRepositoryMock.Object,
                                                       _mapperMock.Object,
                                                       _userManagerMock.Object,
                                                        _chacheServiceMock.Object);
    }



    [Fact]
    public async void Handle_ShouldReturnCreatedProduct_WhenHaveUniqueEmailAndDate()
    {
        //Arrange
        var user = new IdentityUser() { Id = _command.UserId };

        _userManagerMock.Setup(x => x.FindByIdAsync(user.Id))
                     .ReturnsAsync(user);

        var product = new Product();
        _mapperMock.Setup(x => x.Map<Product>(It.IsAny<AddProductCommand>()))
                    .Returns(product);
        _productRepositoryMock.Setup(x => x.IsEmailAndDateUniqueAsync(_command.ManufactureEmail,
                                                                        _command.ProduceDate))
                               .ReturnsAsync(true);

        _productRepositoryMock.Setup(x => x.Add(product));
        _unitOfWorkMock.Setup(x => x.SaveChangesAsync())
                        .Returns(Task.CompletedTask);
        _chacheServiceMock.Setup(x => x.RemoveData("products"));
              

        //Act
        var result = await _commandHandler.Handle(_command, default);
        //Assert
        result.IsError.Should().NotBe(true);
        result.Value.Should().Be(product);
        result.Value.UserId.Should().Be(user.Id);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        _productRepositoryMock.Verify(x => x.Add(product), Times.Once);
        _productRepositoryMock.Verify(x => x.IsEmailAndDateUniqueAsync(_command.ManufactureEmail,
                                                                _command.ProduceDate), Times.Once);
        _userManagerMock.Verify(x => x.FindByIdAsync(_command.UserId), Times.Once);
        _chacheServiceMock.Verify(x => x.RemoveData("products"),
                                     Times.Once);
    }
    [Fact]
    public async Task Handle_ShouldReturnBadRequest_WhenNoUniqueEmailAndDateAsync()
    {
        //Arrange
        var user = new IdentityUser() { Id = _command.UserId };

        _userManagerMock.Setup(x => x.FindByIdAsync(user.Id))
                     .ReturnsAsync(user);

        var product = new Product();
        _mapperMock.Setup(x => x.Map<Product>(It.IsAny<AddProductCommand>()))
                    .Returns(product);

        _productRepositoryMock.Setup(x => x.IsEmailAndDateUniqueAsync(_command.ManufactureEmail,
                                                                        _command.ProduceDate))
                                .ReturnsAsync(false);


        //Act
        var result = await _commandHandler.Handle(_command, default);
        //Assert
        result.IsError.Should().Be(true);
        result.FirstError.Should().Be(Error.Failure(description: "there is a product with this email and produce date "));

    }
    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenUserNotExistAsync()
    {
        //Arrange
        var user = new IdentityUser() { Id = _command.UserId };

        _userManagerMock.Setup(x => x.FindByIdAsync(user.Id))
                     .ReturnsAsync((IdentityUser?)null);

        //Act
        var result = await _commandHandler.Handle(_command, default);
        //Assert
        result.IsError.Should().Be(true);
        result.FirstError.Should().Be(Error.NotFound(description: "something went wrong.. maybe you need to login again"));

    }


}
