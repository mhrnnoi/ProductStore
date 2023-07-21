using MapsterMapper;
using Moq;
using ProductStore.Application.Interfaces.Persistence;

namespace ProductStore.ApplicationTests.Products.Commands.Delete;

public class DeleteProductCommandHandlerTests
{
    private readonly Mock<IProductRepository> _productRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    

    
    [Fact]
    public async void Handle_ShouldDeleteProductAndReturnTrue_WhenProductIsAvailableForUserWithId()
    {
        //Arrange

        //Act
        
        //Assert
      
    }
    [Fact]
    public async Task Handle_ShouldReturnFalse_WhenProductIsNotAvailableForUserWithId()
    {
        //Arrange

        //Act
        
        //Assert

    }


}
