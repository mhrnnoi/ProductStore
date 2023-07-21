namespace ProductStore.Contracts.Products.Requests;

public record AddProductRequest(int UserId,
                                bool IsAvailable,
                                string ManufactureEmail,
                                string ManufacturePhone,
                                DateTime ProduceDate,
                                string Name);

