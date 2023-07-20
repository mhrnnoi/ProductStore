namespace ProductStore.ApplicationTests.Products.Commands.Add;

public class AddProductCommandHandlerTests
{
    [Fact]
    public void Handle_ShouldReturnCreatedProduct_WhenInputIsValid_And_HaveUniqueEmailAndDate()
    {
        //Arrange
        
        //Act

        //Assert
    }
    public void Handle_ShouldReturnBadRequest_WhenInputIsValid_but_NoUniqueEmailAndDate()
    {

    }
    public void Handle_ShouldReturnBadRequest_WhenInputIsNotValid()
    {

    }
  
}
