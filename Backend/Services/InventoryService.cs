using MySql.Data.MySqlClient;
using Backend.Models;

namespace Backend.Services;

public class InventoryService : IInventoryService
{
    private readonly string _connectionString;

    public InventoryService(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("MySqlDb") ?? throw new InvalidOperationException("Connection string 'MySqlDb' not found.");
    }

    public async Task<List<Inventory>> GetAllAsync()
    {
        var list = new List<Inventory>();

        using var conn = new MySqlConnection(_connectionString);
        using var cmd = new MySqlCommand("SELECT * FROM Inventories", conn);

        await conn.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            list.Add(new Inventory
            {
                Id = Convert.ToInt32(reader["Id"]),
                Name = reader["Name"].ToString() ?? string.Empty,
                Description = reader["Description"].ToString() ?? string.Empty,
                Quantity = Convert.ToInt32(reader["Quantity"]),
                Price = Convert.ToDecimal(reader["Price"])
            });
        }

        return list;
    }

    public async Task<Inventory?> GetByIdAsync(int id)
    {
        using var conn = new MySqlConnection(_connectionString);
        using var cmd = new MySqlCommand("SELECT * FROM Inventories WHERE Id = @id", conn);

        cmd.Parameters.AddWithValue("@id", id);

        await conn.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new Inventory
            {
                Id = Convert.ToInt32(reader["Id"]),
                Name = reader["Name"].ToString() ?? string.Empty,
                Description = reader["Description"].ToString() ?? string.Empty,
                Quantity = Convert.ToInt32(reader["Quantity"]),
                Price = Convert.ToDecimal(reader["Price"])
            };
        }

        return null;
    }

    public async Task CreateAsync(Inventory inventory)
    {
        using var conn = new MySqlConnection(_connectionString);

        string sql = @"INSERT INTO Inventories (Name, Description, Quantity, Price)
                       VALUES (@name, @description, @quantity, @price)";

        using var cmd = new MySqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@name", inventory.Name);
        cmd.Parameters.AddWithValue("@description", inventory.Description);
        cmd.Parameters.AddWithValue("@quantity", inventory.Quantity);
        cmd.Parameters.AddWithValue("@price", inventory.Price);

        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task UpdateAsync(int id, Inventory inventory)
    {
        using var conn = new MySqlConnection(_connectionString);

        string sql = @"UPDATE Inventories
                       SET Name = @name, Description = @description, Quantity = @quantity, Price = @price
                       WHERE Id = @id";

        using var cmd = new MySqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@name", inventory.Name);
        cmd.Parameters.AddWithValue("@description", inventory.Description);
        cmd.Parameters.AddWithValue("@quantity", inventory.Quantity);
        cmd.Parameters.AddWithValue("@price", inventory.Price);
        cmd.Parameters.AddWithValue("@id", id);

        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task DeleteAsync(int id)
    {
        using var conn = new MySqlConnection(_connectionString);

        string sql = "DELETE FROM Inventories WHERE Id = @id";

        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@id", id);

        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }
}
