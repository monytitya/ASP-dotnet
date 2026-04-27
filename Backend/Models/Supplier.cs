using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Backend.Models;

[Table("Suppliers")]
public class Supplier
{
    [Key]
    [Column("supplier_id")]
    public int Id { get; set; }

    [Required]
    [Column("name")]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [Column("contact_info")]
    [MaxLength(255)]
    public string? ContactInfo { get; set; }

    [Column("address")]
    [MaxLength(255)]
    public string? Address { get; set; }

    [JsonIgnore]
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
