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

    public DeleteProductCommandHandlerTests()
    {
        _productRepositoryMock = new();
        _mapperMock = new();
        _unitOfWorkMock = new();
        _userManagerMock = new();

        _command = new DeleteProductCommand(It.IsAny<string>(), It.IsAny<int>());

        _commandHandler = new DeleteProductCommandHandler(_unitOfWorkMock.Object,
                                                          _productRepositoryMock.Object,
                                                          _mapperMock.Object,
                                                          _userManagerMock.Object);
    }

    [Fact]
    public async void Handle_ShouldDeleteProductAndReturnTrue_WhenProductIsAvailableForUserWithId()
    {
        //Arrange
        var user = new IdentityUser();
        var product = new Product();
        _userManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                     .ReturnsAsync(user);
        _productRepositoryMock.Setup(x => x.GetUserProductByIdAsync(It.IsAny<string>(), It.IsAny<int>()))
                        .ReturnsAsync(product);
        _productRepositoryMock.Setup(x => x.Remove(product));


        //Act
        var result = await _commandHandler.Handle(_command, default);
        //Assert
        result.IsError.Should().NotBe(true);
        result.Value.Should().Be(true);
        _productRepositoryMock.Verify(x => x.Remove(product), Times.Once);

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

    }
    [Fact]
    public async Task Handle_ShouldReturnNotFound_UserWithIdIsNotExist()
    {
        //Arrange
        _userManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                     .ReturnsAsync((IdentityUser?)null);
        //Act
        var result = await _commandHandler.Handle(_command, default);
        //Assert
        result.IsError.Should().Be(true);
        _productRepositoryMock.Verify(x => x.Remove(It.IsAny<Product>()), Times.Never);


    }


}
