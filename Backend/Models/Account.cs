using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

[Table("accounts")]
public class Account
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("username")]
    public string Username { get; set; } = string.Empty;

    [Required]
    [Column("password")]
    public string Password { get; set; } = string.Empty;

    [Column("email")]
    public string Email { get; set; } = string.Empty;

    [Column("role_id")]
    public int RoleId { get; set; }

    [NotMapped]
    public string? RoleName { get; set; }
}
