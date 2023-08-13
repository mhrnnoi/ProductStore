namespace ProductStore.Contracts.Products.Requests;

public record EditProductRequest(int Id,
                                 int Quantity,
                                 decimal Price,
                                 string Name);

