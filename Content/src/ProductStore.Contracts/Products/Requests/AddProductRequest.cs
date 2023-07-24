using System.ComponentModel.DataAnnotations;

namespace ProductStore.Contracts.Products.Requests;

public record AddProductRequest(bool IsAvailable,
                                string ManufactureEmail,
                                string ManufacturePhone,
                                string Name)
{
    [DataType(DataType.Date, ErrorMessage = "Please enter a valid date.")]
    public DateTime ProduceDate { get; set; }
}

