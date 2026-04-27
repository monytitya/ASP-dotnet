using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Backend.Models;

[Table("Order_Items")]
public class OrderItem
{
    [Key]
    [Column("order_item_id")]
    public int Id { get; set; }

    [Column("order_id")]
    public int OrderId { get; set; }

    [Column("product_id")]
    public int ProductId { get; set; }

    [Column("quantity")]
    public int Quantity { get; set; } = 1;

    [Column("price")]
    public decimal Price { get; set; }


    [JsonIgnore]
    public Order? Order { get; set; }
    public Product? Product { get; set; }
}
