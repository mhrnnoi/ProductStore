using FluentAssertions;
using MapsterMapper;
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

    public AddProductCommandHandlerTests()
    {
        _productRepositoryMock = new();
        _mapperMock = new();
        _unitOfWorkMock = new();

        _command = new AddProductCommand
        (
            true,
            "mehran",
            "16511",
            DateTime.Now,
            "mehran"
            );

        _commandHandler = new AddProductCommandHandler(_unitOfWorkMock.Object,
                                                    _productRepositoryMock.Object,
                                                    _mapperMock.Object);
    }



    [Fact]
    public async void Handle_ShouldReturnCreatedProduct_WhenInputIsValid_And_HaveUniqueEmailAndDate()
    {
        //Arrange
        var product = new Product();
        _mapperMock.Setup(x => x.Map<Product>(It.IsAny<AddProductCommand>()))
                    .Returns(product);

        _productRepositoryMock.Setup(x => x.Add(It.IsAny<Product>()));

        //Act
        var result = await _commandHandler.Handle(_command, default);
        //Assert
        result.IsError.Should().NotBe(true);
        result.Value.Should().Be(product);
    }
    public void Handle_ShouldReturnBadRequest_WhenInputIsValid_but_NoUniqueEmailAndDate()
    {

    }
    public void Handle_ShouldReturnBadRequest_WhenInputIsNotValid()
    {

    }

}
