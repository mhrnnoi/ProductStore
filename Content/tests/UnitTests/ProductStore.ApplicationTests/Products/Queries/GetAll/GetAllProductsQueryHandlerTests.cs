using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Moq;
using ProductStore.Application.Interfaces.Persistence;

namespace ProductStore.ApplicationTests.Products.Queries.GetAll;

public class GetAllProductsQueryHandlerTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly GetAllProductsQuery _query;
    private readonly GetAllProductsQueryHandler _queryHandler;
    private readonly Mock<UserManager<IdentityUser>> _userManager;
    public GetAllProductsQueryHandlerTests()
    {
        _productRepositoryMock = new();
        _mapperMock = new();
        _unitOfWorkMock = new();
        _userManager = new();

        _query = new GetAllProductsQuery(It.IsAny<int>(),
                                          It.IsAny<string>(),
                                          It.IsAny<bool>(),
                                          It.IsAny<string>(),
                                          It.IsAny<string>(),
                                          It.IsAny<DateTime>(),
                                          It.IsAny<string>());

        _queryHandler = new GetAllProductsQueryHandler(_unitOfWorkMock.Object,
                                                       _productRepositoryMock.Object,
                                                       _mapperMock.Object,
                                                       _userManager.Object);
    }

    [Fact]
    public async void Handle_ShouldReturnAllProducts()
    {
        //Arrange

        

        //Act
        
        //Assert
        

    }

}