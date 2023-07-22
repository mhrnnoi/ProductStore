using ErrorOr;
using FluentAssertions;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Moq;
using ProductStore.Application.Features.Products.Queries.GetUserProductsById;
using ProductStore.Application.Interfaces.Persistence;
using ProductStore.Domain.Products.Entities;

namespace ProductStore.ApplicationTests.Products.Queries.GetUserProductsById;

public class GetUserProductsByIdQueryHandlerTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly GetUserProductsByIdQuery _query;
    private readonly GetUserProductsByIdQueryHandler _queryHandler;
    private readonly Mock<UserManager<IdentityUser>> _userManagerMock;
    public GetUserProductsByIdQueryHandlerTests()
    {
        _productRepositoryMock = new();
        _mapperMock = new();
        _unitOfWorkMock = new();
        _userManagerMock = new();

        _query = new GetUserProductsByIdQuery(It.IsAny<string>());

        _queryHandler = new GetUserProductsByIdQueryHandler(_productRepositoryMock.Object,
                                                            _mapperMock.Object,
                                                            _userManagerMock.Object);
    }

    [Fact]
    public async void Handle_ShouldReturnUserProducts()
    {
        //Arrange
        var userId = It.IsAny<string>();
        var user = It.IsAny<IdentityUser>();
        var products = new List<Product>();
        _userManagerMock.Setup(x => x.FindByIdAsync(userId))
                    .ReturnsAsync(user);
        _productRepositoryMock.Setup(x => x.GetUserProductsAsync(userId))
                                .ReturnsAsync(products);

        //Act
        var result = await _queryHandler.Handle(_query, default);

        //Assert

        result.IsError.Should().Be(false);
        result.Value.Should().BeOfType(products.GetType());
    }
    [Fact]
    public async void Handle_ShouldReturnNotFound_WhenUserIsNotExist()
    {
        //Arrange
        var userId = It.IsAny<string>();
        _userManagerMock.Setup(x => x.FindByIdAsync(userId))
                    .ReturnsAsync((IdentityUser?)null);

        //Act
        var result = await _queryHandler.Handle(_query, default);

        //Assert

        result.IsError.Should().Be(true);
        result.FirstError.Should().Be(Error.NotFound());
    }

}