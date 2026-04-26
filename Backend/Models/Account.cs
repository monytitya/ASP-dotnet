namespace Backend.Models;

public class Account
{
    public int    Id       { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Email    { get; set; } = string.Empty;
    public int    RoleId   { get; set; }

    // Read-only: populated on GET, ignored on POST/PUT
    public string? RoleName { get; set; }
}
