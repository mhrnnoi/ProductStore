namespace ProductStore.Domain.Products.Entities;

public class Product
{
    public int Id { get; set; }
    public bool IsAvailable { get; set; }
    public string ManufactureEmail { get; set; }
    public string ManufacturePhone { get; set; }
    public DateTime ProduceDate { get; set; }
    public string Name { get; set; }

}