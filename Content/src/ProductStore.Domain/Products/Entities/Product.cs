using ErrorOr;
using ProductStore.Domain.Common.Errors;
using ProductStore.Domain.Primitives;

namespace ProductStore.Domain.Products.Entities;

public sealed class Product : Entity
{
    public string UserId { get; }
    public string Name { get; }
    public int Quantity { get; private set; }
    public decimal Price { get; private set; }

    private Product(string userId,
                   string name,
                   int quantity,
                   decimal price) : base()
    {
        UserId = userId;
        Name = name;
        Quantity = quantity;
        Price = price;
    }

    public ErrorOr<Product> UpdateQuantity(int quantity)
    {
        if (quantity < 0)
            return Errors.Product.QuantityLowerThanZero;
        this.Quantity = quantity;
        return this;
    }
    public ErrorOr<Product> UpdatePrice(decimal price)
    {
        if (price < 0)
            return Errors.Product.PriceLowerThanZero;
        this.Price = price;
        return this;
    }

    public static ErrorOr<Product> Create(string userId,
                                 string name,
                                 int quantity,
                                 decimal price)
    {
        if (quantity < 0)
            return Errors.Product.QuantityLowerThanZero;
        if (price < 0)
            return Errors.Product.PriceLowerThanZero;
        if (string.IsNullOrWhiteSpace(userId) || userId.Length < 36)
            return Errors.Product.UserIdMinimumCharacter;
        if (string.IsNullOrWhiteSpace(name) || name.Length < 2)
            return Errors.Product.NameMinimumCharacter;


        return new Product(userId,
                           name,
                           quantity,
                           price);

    }

}