using FluentAssertions;
using ProductStore.Domain.Products.Entities;

namespace ProductStore.DomainTests.Products.Entities;

public class ProductTests
{
    [Fact]
    public void Product_Properties_ShouldBeSetCorrectly()
    {
        //Arrange

        var isAvailable = true;
        var manufactureEmail = "Mehran@email.com";
        var manufacturePhone = "0123456789";
        var produceDate = DateTime.Now;
        var name = "book";

        //Act

        var product = new Product()
        {
            IsAvailable = isAvailable,
            ManufactureEmail = manufactureEmail,
            ManufacturePhone = manufacturePhone,
            ProduceDate = produceDate,
            Name = name
        };

        //Assert
        product.Name.Should().Be(name);
        product.IsAvailable.Should().Be(isAvailable);
        product.ManufactureEmail.Should().Be(manufactureEmail);
        product.ProduceDate.Should().Be(produceDate);
        product.ManufacturePhone.Should().Be(manufacturePhone);

        


    }


}