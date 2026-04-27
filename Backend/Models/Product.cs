using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Backend.Models;

[Table("Products")]
public class Product
{
    [Key]
    [Column("product_id")]
    public int Id { get; set; }

    [Required]
    [Column("name")]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [Column("description")]
    public string? Description { get; set; }

    [Column("category_id")]
    public int CategoryId { get; set; }

    [Column("price")]
    public decimal Price { get; set; } = 0.00m;

    [Column("stock_quantity")]
    public int StockQuantity { get; set; } = 0;

    [Column("supplier_id")]
    public int SupplierId { get; set; }

    // Navigation properties
    public Category? Category { get; set; }
    public Supplier? Supplier { get; set; }
    
    [JsonIgnore]
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    
    [JsonIgnore]
    public ICollection<PurchaseItem> PurchaseItems { get; set; } = new List<PurchaseItem>();
}
