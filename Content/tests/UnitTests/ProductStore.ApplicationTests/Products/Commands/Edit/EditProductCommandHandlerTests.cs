using FluentAssertions;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Moq;
using ProductStore.Application.Features.Products.Commands.Edit;
using ProductStore.Application.Interfaces.Persistence;
using ProductStore.Domain.Products.Entities;

namespace ProductStore.ApplicationTests.Products.Commands.Edit;


public class EditProductCommandHandlerTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly EditProductCommand _command;
    private readonly EditProductCommandHandler _commandHandler;
    private readonly Mock<UserManager<IdentityUser>> _userManagerMock;
    public EditProductCommandHandlerTests()
    {
        _productRepositoryMock = new();
        _mapperMock = new();
        _unitOfWorkMock = new();
        _userManagerMock = new();

        _command = new EditProductCommand(It.IsAny<int>(),
                                          It.IsAny<string>(),
                                          It.IsAny<bool>(),
                                          It.IsAny<string>(),
                                          It.IsAny<string>(),
                                          It.IsAny<DateTime>(),
                                          It.IsAny<string>());

        _commandHandler = new EditProductCommandHandler(_unitOfWorkMock.Object,
                                                       _productRepositoryMock.Object,
                                                       _mapperMock.Object,
                                                       _userManagerMock.Object);
    }

    [Fact]
    public async void Handle_ShouldEditProductAndReturnEditedProduct_WhenProductIsAvailableForUserWithIdAndStillHaveUniqueEmailAndDateAndUserExists()
    {
        //Arrange
        var user = new IdentityUser() { Id = It.IsAny<string>() };

        _userManagerMock.Setup(x => x.FindByIdAsync(user.Id))
                     .ReturnsAsync(user);

        _productRepositoryMock.Setup(x => x.GetUserProductByIdAsync(_command.UserId, _command.id))
                     .ReturnsAsync(It.IsAny<Product>());
        _productRepositoryMock.Setup(x => x.IsEmailAndDateUniqueAsync(_command.ManufactureEmail,
                                                                        _command.ProduceDate))
                               .ReturnsAsync(true);
        _mapperMock.Setup(x => x.Map<Product>(It.IsAny<EditProductCommand>()))
                    .Returns(It.IsAny<Product>());
        var product = new Product();
        _productRepositoryMock.Setup(x => x.Update(product));
        _unitOfWorkMock.Setup(x => x.SaveChangesAsync())
                        .Returns(Task.CompletedTask);

        //Act
        var result = await _commandHandler.Handle(_command, default);
        //Assert
        result.IsError.Should().NotBe(true);
        result.Value.Should().Be(product);
        result.Value.UserId.Should().Be(user.Id);


    }
    [Fact]
    public async void Handle_ShouldReturnNotFound_WhenProductIsNotAvailableForUserWithId()
    {
        //Arrange
        var user = new IdentityUser() { Id = It.IsAny<string>() };

        _userManagerMock.Setup(x => x.FindByIdAsync(user.Id))
                     .ReturnsAsync(user);

        _productRepositoryMock.Setup(x => x.GetUserProductByIdAsync(_command.UserId, _command.id))
                     .ReturnsAsync((Product?)null);


        //Act
        var result = await _commandHandler.Handle(_command, default);
        //Assert
        result.IsError.Should().Be(true);

    }
    [Fact]
    public async void Handle_ShouldReturnNotFound_WhenUserWithIdIsNotExist()
    {
        //Arrange

        _userManagerMock.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
                     .ReturnsAsync((IdentityUser?)null);

        //Act
        var result = await _commandHandler.Handle(_command, default);
        //Assert
        result.IsError.Should().Be(true);

    }
    [Fact]
    public async void Handle_ShouldReturnBadRequest_WhenNoUniqueEmailAndDate()
    {
        //Arrange
        var user = new IdentityUser() { Id = It.IsAny<string>() };

        _userManagerMock.Setup(x => x.FindByIdAsync(user.Id))
                     .ReturnsAsync(user);

        _productRepositoryMock.Setup(x => x.GetUserProductByIdAsync(_command.UserId, _command.id))
                     .ReturnsAsync(It.IsAny<Product>());
        _productRepositoryMock.Setup(x => x.IsEmailAndDateUniqueAsync(_command.ManufactureEmail,
                                                                        _command.ProduceDate))
                               .ReturnsAsync(false);

        //Act
        var result = await _commandHandler.Handle(_command, default);
        //Assert
        result.IsError.Should().Be(true);
        result.Value.UserId.Should().Be(user.Id);

    }

}