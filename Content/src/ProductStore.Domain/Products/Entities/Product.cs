using System.ComponentModel.DataAnnotations;

namespace ProductStore.Domain.Products.Entities;

public class Product
{
    private Product(string userId,
    bool isAvailable,
    string manufactureEmail,
    string manufacturePhone,
    DateTime produceDate,
    string name)
    {
        UserId = userId;
        IsAvailable = isAvailable;
        ManufactureEmail = manufactureEmail;
        ManufacturePhone = manufacturePhone;
        ProduceDate = produceDate;
        Name = name;
    }
    public int Id { get; private set; }
    public string UserId { get; private set; } = string.Empty;
    public bool IsAvailable { get; set; }
    public string ManufactureEmail { get; set; } = string.Empty;
    public string ManufacturePhone { get; set; } = string.Empty;
    [DataType(DataType.Date)]
    public DateTime ProduceDate { get; set; }
    public string Name { get; set; } = string.Empty;

    public static Product Create(string userId,
    bool isAvailable,
    string manufactureEmail,
    string manufacturePhone,
    DateTime produceDate,
    string name)
    {
        return new Product(userId,
                           isAvailable,
                           manufactureEmail,
                           manufacturePhone,
                           produceDate,
                           name);
    }
    // public static Product Update(this Product product,
    // bool isAvailable,
    // string manufactureEmail,
    // string manufacturePhone,
    // DateTime produceDate,
    // string name)
    // {
    //     product.IsAvailable = isAvailable;
    //     product.ProduceDate = produceDate;
    //     product.Name = name;
    //     product.ManufactureEmail = manufactureEmail;
    //     product.ManufacturePhone = manufacturePhone;
    //     return product;
    // }
}