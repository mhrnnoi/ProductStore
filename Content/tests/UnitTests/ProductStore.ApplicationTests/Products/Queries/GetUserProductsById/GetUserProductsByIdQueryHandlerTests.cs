using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Moq;
using ProductStore.Application.Interfaces.Persistence;

namespace ProductStore.ApplicationTests.Products.Queries.GetUserProductsById;

public class GetUserProductsByIdQueryHandlerTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly GetUserProductsByIdQuery _query;
    private readonly GetUserProductsByIdQueryHandler _queryHandler;
    private readonly Mock<UserManager<IdentityUser>> _userManager;
    public GetUserProductsByIdQueryHandlerTests()
    {
        _productRepositoryMock = new();
        _mapperMock = new();
        _unitOfWorkMock = new();
        _userManager = new();

        _query = new GetUserProductsByIdQuery(It.IsAny<int>(),
                                          It.IsAny<string>(),
                                          It.IsAny<bool>(),
                                          It.IsAny<string>(),
                                          It.IsAny<string>(),
                                          It.IsAny<DateTime>(),
                                          It.IsAny<string>());

        _queryHandler = new GetUserProductsByIdQueryHandler(_unitOfWorkMock.Object,
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