using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

[Table("roles")]
public class Role
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("name")]
    public string Name { get; set; } = string.Empty;
}
