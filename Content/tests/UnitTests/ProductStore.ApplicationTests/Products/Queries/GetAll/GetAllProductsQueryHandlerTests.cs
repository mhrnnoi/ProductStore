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
    public GetAllProductsQueryHandlerTests()
    {
        _productRepositoryMock = new();
        _mapperMock = new();
        _unitOfWorkMock = new();
        _userManagerMock = new();

        _query = new GetAllProductsQuery();

        _queryHandler = new GetAllProductsQueryHandler(_productRepositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async void Handle_ShouldReturnAllProducts()
    {
        //Arrange
        var products = new List<Product>();
        _productRepositoryMock.Setup(x => x.GetAllAsync())
                                .ReturnsAsync(products);

        //Act
        var result = await _queryHandler.Handle(_query, default);

        //Assert

        result.IsError.Should().Be(false);
        result.Value.Should().BeOfType(products.GetType());
        _productRepositoryMock.Verify(x => x.GetAllAsync(), Times.Once);


    }

}