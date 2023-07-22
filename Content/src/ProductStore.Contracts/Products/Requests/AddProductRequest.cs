namespace ProductStore.Contracts.Products.Requests;

public record AddProductRequest(bool IsAvailable,
                                string ManufactureEmail,
                                string ManufacturePhone,
                                DateTime ProduceDate,
                                string Name);

