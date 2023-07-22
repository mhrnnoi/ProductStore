using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Moq;
using ProductStore.Application.Interfaces.Persistence;

namespace ProductStore.ApplicationTests.Products.Queries.GetUserProducts;

public class GetUserProductsQueryHandlerTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly GetUserProductsQuery _query;
    private readonly GetUserProductsQueryHandler _queryHandler;
    private readonly Mock<UserManager<IdentityUser>> _userManager;
    public GetUserProductsQueryHandlerTests()
    {
        _productRepositoryMock = new();
        _mapperMock = new();
        _unitOfWorkMock = new();
        _userManager = new();

        _query = new GetUserProductsQuery(It.IsAny<int>(),
                                          It.IsAny<string>(),
                                          It.IsAny<bool>(),
                                          It.IsAny<string>(),
                                          It.IsAny<string>(),
                                          It.IsAny<DateTime>(),
                                          It.IsAny<string>());

        _queryHandler = new GetUserProductsQueryHandler(_unitOfWorkMock.Object,
                                                       _productRepositoryMock.Object,
                                                       _mapperMock.Object,
                                                       _userManager.Object);
    }

    [Fact]
    public async void Handle_ShouldReturnUserProducts()
    {
        //Arrange

        

        //Act
        
        //Assert
        

    }
    [Fact]
    public async void Handle_ShouldReturnNotFound_WhenUserIsNotExist()
    {
        //Arrange

        

        //Act
        
        //Assert
        

    }

}