using FluentAssertions;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Moq;
using ProductStore.Application.Features.Products.Queries.GetAll;
using ProductStore.Application.Interfaces.Persistence;
using ProductStore.Domain.Products.Entities;

namespace ProductStore.ApplicationTests.Products.Queries.GetAll;

public class GetAllProductsQueryHandlerTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly GetAllProductsQuery _query;
    private readonly GetAllProductsQueryHandler _queryHandler;
    private readonly Mock<UserManager<IdentityUser>> _userManagerMock;
    private readonly Mock<IUserStore<IdentityUser>> _userStoreMock;
    private readonly Mock<ICacheService> _cacheService;

    public GetAllProductsQueryHandlerTests()
    {
        _productRepositoryMock = new();
        _cacheService = new();
        _mapperMock = new();
        _unitOfWorkMock = new();
        _userStoreMock = new();

        _userManagerMock = new(_userStoreMock.Object, null, null, null, null, null, null, null, null);




        _query = new GetAllProductsQuery();

        _queryHandler = new GetAllProductsQueryHandler(_productRepositoryMock.Object, _mapperMock.Object, _cacheService.Object);
    }

    [Fact]
    public async void Handle_ShouldReturnAllProducts_WhenOkCache()
    {
        //Arrange
        var products = new List<Product>() { new Product() };
        var key = "products";
        _cacheService.Setup(x => x.GetData<List<Product>>(key))
                           .Returns(products);

        //Act
        var result = await _queryHandler.Handle(_query, default);

        //Assert

        result.IsError.Should().Be(false);
        result.Value.Should().BeOfType(products.GetType());
        _productRepositoryMock.Verify(x => x.GetAllAsync(), Times.Never);
        _cacheService.Verify(x => x.GetData<List<Product>>(key), Times.Once);
        _cacheService.Verify(x => x.SetData<List<Product>>(key, It.IsAny<List<Product>>(), It.IsAny<DateTimeOffset>()), Times.Never);


    }
    [Fact]
    public async void Handle_ShouldReturnAllProducts_WhenNoCache()
    {
        //Arrange
        var products = new List<Product>() { new Product() };
        var key = "products";
        var emptyList = new List<Product>() { };
        var productsInRedis = _cacheService.Setup(x => x.GetData<List<Product>>(key))
                        .Returns(emptyList);
        _productRepositoryMock.Setup(x => x.GetAllAsync())
               .ReturnsAsync(products);

        _cacheService.Setup(x => x.SetData<List<Product>>(key, products, It.IsAny<DateTimeOffset>()))
           .Returns(true);

        //Act
        var result = await _queryHandler.Handle(_query, default);

        //Assert

        result.IsError.Should().Be(false);
        result.Value.Should().BeOfType(products.GetType());
        _cacheService.Verify(x => x.GetData<List<Product>>(key), Times.Once);
        _cacheService.Verify(x => x.SetData<List<Product>>(key, products, It.IsAny<DateTimeOffset>()), Times.Once);
        _productRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);


    }

}