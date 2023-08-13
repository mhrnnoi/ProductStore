namespace ProductStore.Contracts.Products.Requests;

public record UpdateQuantityRequest(int Id,
                                 int Quantity,
                                 string Name);

