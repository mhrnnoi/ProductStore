namespace ProductStore.Contracts.Products.Requests;

public record UpdatePriceRequest(int Id,
                                 decimal Price,
                                 string Name);

