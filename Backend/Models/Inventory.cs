using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

[Table("Inventories")]
public class Inventory
{
    [Key]
    [Column("Id")]
    public int Id { get; set; }

    [Required]
    [Column("Name")]
    public string Name { get; set; } = string.Empty;

    [Column("Description")]
    public string? Description { get; set; }

    [Column("Quantity")]
    public int Quantity { get; set; }

    [Column("Price")]
    public decimal Price { get; set; }
}
