using ErrorOr;
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
    private readonly Mock<IUserStore<IdentityUser>> _userStoreMock;
    private readonly Mock<ICacheService> _chacheServiceMock;

    public EditProductCommandHandlerTests()
    {
        _productRepositoryMock = new();
        _mapperMock = new();
        _unitOfWorkMock = new();
        _userStoreMock = new();
        _chacheServiceMock = new();


        _userManagerMock = new(_userStoreMock.Object, null, null, null, null, null, null, null, null);




        _command = new EditProductCommand(It.IsAny<int>(),
                                          It.IsAny<string>(),
                                          It.IsAny<bool>(),
                                          "email",
                                          "phone",
                                          new DateTime(2000, 12, 26),
                                          It.IsAny<string>());

        _commandHandler = new EditProductCommandHandler(_unitOfWorkMock.Object,
                                                       _productRepositoryMock.Object,
                                                       _mapperMock.Object,
                                                       _userManagerMock.Object,
                                                       _chacheServiceMock.Object);
    }

    [Fact]
    public async void Handle_ShouldEditProductAndReturnEditedProduct_WhenProductIsAvailableForUserWithIdAndStillHaveUniqueEmailAndDateAndUserExists()
    {
        //Arrange
        var user = new IdentityUser() { Id = _command.UserId };

        _userManagerMock.Setup(x => x.FindByIdAsync(user.Id))
                        .ReturnsAsync(user);

        var product = new Product()
        {
            Id = _command.Id,
            UserId = _command.UserId
        };

        _productRepositoryMock.Setup(x => x.GetUserProductByIdAsync(_command.UserId, _command.Id))
                     .ReturnsAsync(product);

        _productRepositoryMock.Setup(x => x.IsEmailAndDateUniqueAsync(_command.ManufactureEmail,
                                                                        _command.ProduceDate)).ReturnsAsync(true);

        _mapperMock.Setup(x => x.Map<Product>(It.IsAny<EditProductCommand>()))
                    .Returns(product);

        _productRepositoryMock.Setup(x => x.Update(product));
        _unitOfWorkMock.Setup(x => x.SaveChangesAsync())
                        .Returns(Task.CompletedTask);
        _chacheServiceMock.Setup(x => x.RemoveData("products"));


        //Act
        var result = await _commandHandler.Handle(_command, default);
        //Assert
        result.IsError.Should().NotBe(true);
        result.Value.Should().Be(product);
        result.Value.UserId.Should().Be(user.Id);
        _productRepositoryMock.Verify(x => x.GetUserProductByIdAsync(user.Id, product.Id), Times.Once);
        _userManagerMock.Verify(x => x.FindByIdAsync(user.Id), Times.Once);
        _productRepositoryMock.Verify(x => x.IsEmailAndDateUniqueAsync(_command.ManufactureEmail, _command.ProduceDate), Times.Once);
        _productRepositoryMock.Verify(x => x.Update(product), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        _chacheServiceMock.Verify(x => x.RemoveData("products"),
                                     Times.Once);



    }


    [Fact]
    public async void Handle_ShouldReturnNotFound_WhenProductIsNotAvailableForUserWithId()
    {
        //Arrange
        var user = new IdentityUser() { Id = It.IsAny<string>() };

        _userManagerMock.Setup(x => x.FindByIdAsync(user.Id))
                     .ReturnsAsync(user);

        _productRepositoryMock.Setup(x => x.GetUserProductByIdAsync(_command.UserId, _command.Id))
                     .ReturnsAsync((Product?)null);


        //Act
        var result = await _commandHandler.Handle(_command, default);
        //Assert
        result.IsError.Should().Be(true);
        result.FirstError.Should().Be(Error.NotFound(description: "product with this id is not exist in your product list.."));


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
        result.FirstError.Should().Be(Error.NotFound(description: "something went wrong.. maybe you need to login again"));

    }
    [Fact]
    public async void Handle_ShouldReturnBadRequest_WhenNoUniqueEmailAndDate()
    {
        //Arrange
        var user = new IdentityUser() { Id = It.IsAny<string>() };

        _userManagerMock.Setup(x => x.FindByIdAsync(user.Id))
                     .ReturnsAsync(user);

        var product = new Product() { UserId = user.Id, ManufactureEmail = "new one", ProduceDate = new DateTime(2000, 12, 25) };
        _productRepositoryMock.Setup(x => x.GetUserProductByIdAsync(_command.UserId, _command.Id))
                     .ReturnsAsync(product);
        _productRepositoryMock.Setup(x => x.IsEmailAndDateUniqueAsync(_command.ManufactureEmail,
                                                                        _command.ProduceDate))
                               .ReturnsAsync(false);

        //Act
        var result = await _commandHandler.Handle(_command, default);
        //Assert
        result.IsError.Should().Be(true);
        result.FirstError.Should().Be(Error.Failure(description: "there is a product with this email and produce date "));

    }

}