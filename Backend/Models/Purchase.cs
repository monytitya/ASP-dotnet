using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

[Table("Purchases")]
public class Purchase
{
    [Key]
    [Column("purchase_id")]
    public int Id { get; set; }

    [Column("supplier_id")]
    public int SupplierId { get; set; }

    [Column("purchase_date")]
    public DateTime PurchaseDate { get; set; } = DateTime.UtcNow.Date;

    // Navigation properties
    public Supplier? Supplier { get; set; }
    public ICollection<PurchaseItem> PurchaseItems { get; set; } = new List<PurchaseItem>();
}
