using System.ComponentModel.DataAnnotations;

namespace ProductStore.Domain.Products.Entities;

public class Product
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public bool IsAvailable { get; set; }
    public string ManufactureEmail { get; set; }
    public string ManufacturePhone { get; set; }
    [DataType(DataType.Date)]
    public DateTime ProduceDate { get; set; }
    public string Name { get; set; }

}