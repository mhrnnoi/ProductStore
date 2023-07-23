namespace ProductStore.Contracts.Products.Requests;

public record EditProductRequest(int id,
                                 bool IsAvailable,
                                 string ManufactureEmail,
                                 string ManufacturePhone,
                                 DateTime ProduceDate,
                                 string Name);