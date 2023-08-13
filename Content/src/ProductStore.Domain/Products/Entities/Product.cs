using ProductStore.Domain.Primitives;

namespace ProductStore.Domain.Products.Entities;

public sealed class Product : Entity
{
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
    public string UserId { get; init; }
    public string Name { get; init; } = string.Empty;
    public int Quantity { get; private set; }
    public decimal Price { get; private set; }

    public Product UpdateQuantity(int quantity)
    {
        this.Quantity = quantity;
        return this;
    }
    public Product UpdatePrice(decimal price)
    {
        this.Price = price;
        return this;
    }

    public static Product Create(string userId,
                                 string name,
                                 int quantity,
                                 decimal price)
    {

        return new Product(userId,
                           name,
                           quantity,
                           price);

    }

}