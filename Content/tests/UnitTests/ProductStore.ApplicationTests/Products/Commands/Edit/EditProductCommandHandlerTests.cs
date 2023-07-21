namespace ProductStore.ApplicationTests.Products.Commands.Edit;


public class EditProductCommandHandlerTests
{
    public EditProductCommandHandlerTests()
    {

    }

    [Fact]
    public async void Handle_ShouldEditProductAndReturnEditedProduct_WhenProductIsAvailableForUserWithIdAndStillHaveUniqueEmailAndDateAndUserExists()
    {
        //Arrange


        //Act


        //Assert


    }
    [Fact]
    public async void Handle_ShouldReturnNotFound_WhenProductIsNotAvailableForUserWithId()
    {
        //Arrange


        //Act


        //Assert


    }
    [Fact]
    public async void Handle_ShouldReturnNotFound_WhenUserWithIdIsNotExist()
    {
        //Arrange


        //Act


        //Assert


    }
    [Fact]
    public async void Handle_ShouldReturnBadRequest_WhenNoUniqueEmailAndDate()
    {
        //Arrange


        //Act


        //Assert


    }

}