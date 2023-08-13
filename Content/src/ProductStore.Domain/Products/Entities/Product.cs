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

    public static ErrorOr<Product> Create(string userId,
                                 string name,
                                 int quantity,
                                 decimal price)
    {
        if (quantity < 0)
        {
            return Errors.Product.
        }
        if (price < 0)
        {
            
        }
        if (userId.Length < 50)
        {
            
        }


        return new Product(userId,
                           name,
                           quantity,
                           price);

    }

}