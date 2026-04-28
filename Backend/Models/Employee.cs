using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

[Table("Employees")]
public class Employee
{
    [Key]
    [Column("Id")]
    public int Id { get; set; }

    [Required]
    [Column("Name")]
    public string Name { get; set; } = string.Empty;

    [Column("Email")]
    public string Email { get; set; } = string.Empty;

    [Column("Salary")]
    public decimal Salary { get; set; }

    [Column("image_url")]
    public string? ImageUrl { get; set; }
}
