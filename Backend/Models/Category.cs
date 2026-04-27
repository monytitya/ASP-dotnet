using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Backend.Models;

[Table("Categories")]
public class Category
{
    [Key]
    [Column("category_id")]
    public int Id { get; set; }

    [Required]
    [Column("category_name")]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [JsonIgnore]
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
