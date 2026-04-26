using MySql.Data.MySqlClient;
using Backend.Models;

namespace Backend.Services;

public class RoleService : IRoleService
{
    private readonly string _connectionString;

    public RoleService(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("MySqlDb")
            ?? throw new InvalidOperationException("Connection string 'MySqlDb' not found.");
    }

    public async Task<List<Role>> GetAllAsync()
    {
        var list = new List<Role>();
        using var conn = new MySqlConnection(_connectionString);
        using var cmd  = new MySqlCommand("SELECT Id, Name FROM Roles", conn);
        await conn.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
            list.Add(new Role { Id = Convert.ToInt32(reader["Id"]), Name = reader["Name"].ToString()! });
        return list;
    }

    public async Task<Role?> GetByIdAsync(int id)
    {
        using var conn = new MySqlConnection(_connectionString);
        using var cmd  = new MySqlCommand("SELECT Id, Name FROM Roles WHERE Id = @id", conn);
        cmd.Parameters.AddWithValue("@id", id);
        await conn.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync())
            return new Role { Id = Convert.ToInt32(reader["Id"]), Name = reader["Name"].ToString()! };
        return null;
    }

    public async Task CreateAsync(Role role)
    {
        using var conn = new MySqlConnection(_connectionString);
        using var cmd  = new MySqlCommand("INSERT INTO Roles (Name) VALUES (@name)", conn);
        cmd.Parameters.AddWithValue("@name", role.Name);
        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task UpdateAsync(int id, Role role)
    {
        using var conn = new MySqlConnection(_connectionString);
        using var cmd  = new MySqlCommand("UPDATE Roles SET Name = @name WHERE Id = @id", conn);
        cmd.Parameters.AddWithValue("@name", role.Name);
        cmd.Parameters.AddWithValue("@id", id);
        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task DeleteAsync(int id)
    {
        using var conn = new MySqlConnection(_connectionString);
        using var cmd  = new MySqlCommand("DELETE FROM Roles WHERE Id = @id", conn);
        cmd.Parameters.AddWithValue("@id", id);
        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }
}
