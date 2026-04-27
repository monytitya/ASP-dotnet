using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Backend.Models;

[Table("Purchase_Items")]
public class PurchaseItem
{
    [Key]
    [Column("purchase_item_id")]
    public int Id { get; set; }

    [Column("purchase_id")]
    public int PurchaseId { get; set; }

    [Column("product_id")]
    public int ProductId { get; set; }

    [Column("quantity")]
    public int Quantity { get; set; } = 1;

    [Column("cost_price")]
    public decimal CostPrice { get; set; }

    // Navigation properties
    [JsonIgnore]
    public Purchase? Purchase { get; set; }
    public Product? Product { get; set; }
}
