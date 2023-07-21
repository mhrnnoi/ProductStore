using ErrorOr;
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
            "1",
            true,
            "mehran",
            "16511",
            DateTime.Now.Date,
            "mehran"
            );

        _commandHandler = new AddProductCommandHandler(_unitOfWorkMock.Object,
                                                    _productRepositoryMock.Object,
                                                    _mapperMock.Object);
    }



    [Fact]
    public async void Handle_ShouldReturnCreatedProduct_WhenHaveUniqueEmailAndDate()
    {
        //Arrange
        var product = new Product();
        _mapperMock.Setup(x => x.Map<Product>(It.IsAny<AddProductCommand>()))
                    .Returns(product);
        _productRepositoryMock.Setup(x => x.IsEmailAndDateUniqueAsync(_command.ManufactureEmail,
                                                                        _command.ProduceDate))
                               .ReturnsAsync(true);

        _productRepositoryMock.Setup(x => x.Add(It.IsAny<Product>()));

        //Act
        var result = await _commandHandler.Handle(_command, default);
        //Assert
        result.IsError.Should().NotBe(true);
        result.Value.Should().Be(product);
    }
    [Fact]
    public async Task Handle_ShouldReturnBadRequest_WhenNoUniqueEmailAndDateAsync()
    {
        //Arrange
        var product = new Product();
        _mapperMock.Setup(x => x.Map<Product>(It.IsAny<AddProductCommand>()))
                    .Returns(product);

        _productRepositoryMock.Setup(x => x.IsEmailAndDateUniqueAsync(_command.ManufactureEmail,
                                                                        _command.ProduceDate))
                                .ReturnsAsync(false);

        _productRepositoryMock.Setup(x => x.Add(It.IsAny<Product>()));

        //Act
        var result = await _commandHandler.Handle(_command, default);
        //Assert
        result.IsError.Should().Be(true);
        result.FirstError.Should().Be(Error.Failure());

    }


}
