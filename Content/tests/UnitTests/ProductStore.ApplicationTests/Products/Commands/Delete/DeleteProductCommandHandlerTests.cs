using ErrorOr;
using FluentAssertions;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Moq;
using ProductStore.Application.Features.Products.Commands.Delete;
using ProductStore.Application.Interfaces.Persistence;
using ProductStore.Domain.Products.Entities;

namespace ProductStore.ApplicationTests.Products.Commands.Delete;

public class DeleteProductCommandHandlerTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;

    private readonly DeleteProductCommand _command;
    private readonly DeleteProductCommandHandler _commandHandler;
    private readonly Mock<UserManager<IdentityUser>> _userManagerMock;
    private readonly Mock<IUserStore<IdentityUser>> _userStoreMock;
    private readonly Mock<ICacheService> _chacheServiceMock;


    public DeleteProductCommandHandlerTests()
    {
        _productRepositoryMock = new();
        _mapperMock = new();
        _unitOfWorkMock = new();
        _chacheServiceMock = new();

        _userStoreMock = new();
        _userManagerMock = new(_userStoreMock.Object, null, null, null, null, null, null, null, null);



        _command = new DeleteProductCommand(It.IsAny<string>(), It.IsAny<int>());

        _commandHandler = new DeleteProductCommandHandler(_unitOfWorkMock.Object,
                                                          _productRepositoryMock.Object,
                                                          _mapperMock.Object,
                                                          _userManagerMock.Object,
                                                          _chacheServiceMock.Object);
    }

    [Fact]
    public async void Handle_ShouldDeleteProductAndReturnTrue_WhenProductIsAvailableForUserWithId()
    {
        //Arrange
        var user = new IdentityUser() { Id = _command.UserId };
        var product = new Product() { Id = It.IsAny<int>() };
        _userManagerMock.Setup(x => x.FindByIdAsync(user.Id))
                     .ReturnsAsync(user);
        _productRepositoryMock.Setup(x => x.GetUserProductByIdAsync(user.Id, product.Id))
                        .ReturnsAsync(product);
        _productRepositoryMock.Setup(x => x.Remove(product));
        _unitOfWorkMock.Setup(x => x.SaveChangesAsync())
                                .Returns(Task.CompletedTask);

        //Act
        var result = await _commandHandler.Handle(_command, default);
        //Assert
        result.IsError.Should().NotBe(true);
        result.Value.Should().Be(true);
        _productRepositoryMock.Verify(x => x.Remove(product), Times.Once);
        _userManagerMock.Verify(x => x.FindByIdAsync(user.Id), Times.Once);
        _productRepositoryMock.Verify(x => x.GetUserProductByIdAsync(user.Id, product.Id),
                                                                             Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        _chacheServiceMock.Verify(x => x.RemoveData("products"),
                                     Times.Once);


    }
    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenProductIsNotAvailableForUserWithId()
    {
        //Arrange
        var user = new IdentityUser();

        _userManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                     .ReturnsAsync(user);
        _productRepositoryMock.Setup(x => x.GetUserProductByIdAsync(It.IsAny<string>(), It.IsAny<int>()))
                        .ReturnsAsync((Product?)null);

        //Act
        var result = await _commandHandler.Handle(_command, default);
        //Assert
        result.IsError.Should().Be(true);
        _productRepositoryMock.Verify(x => x.Remove(It.IsAny<Product>()), Times.Never);
        result.FirstError.Should().Be(Error.NotFound(description: "product with this id is not exist in your product list.."));

    }
    [Fact]
    public async Task Handle_ShouldReturnNotFound_WhenUserWithIdIsNotExist()
    {
        //Arrange
        _userManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                     .ReturnsAsync((IdentityUser?)null);
        //Act
        var result = await _commandHandler.Handle(_command, default);
        //Assert
        result.IsError.Should().Be(true);
        result.FirstError.Should().Be(Error.NotFound(description: "something went wrong.. maybe you need to login again"));
        _productRepositoryMock.Verify(x => x.Remove(It.IsAny<Product>()), Times.Never);


    }


}
