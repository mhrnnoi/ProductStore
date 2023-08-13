namespace ProductStore.Contracts.Products.Requests;

public record AddProductRequest(int Quantity,
                                decimal Price,
                                string Name);


