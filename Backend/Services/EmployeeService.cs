using MySql.Data.MySqlClient;
using Backend.Models;

namespace Backend.Services;

public class EmployeeService : IEmployeeService
{
    private readonly string _connectionString;

    public EmployeeService(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("MySqlDb") ?? throw new InvalidOperationException("Connection string 'MySqlDb' not found.");
    }

    public async Task<List<Employee>> GetAllAsync()
    {
        var list = new List<Employee>();

        using var conn = new MySqlConnection(_connectionString);
        using var cmd = new MySqlCommand("SELECT * FROM Employees", conn);

        await conn.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            list.Add(new Employee
            {
                Id = Convert.ToInt32(reader["Id"]),
                Name = reader["Name"].ToString() ?? string.Empty,
                Email = reader["Email"].ToString() ?? string.Empty,
                Salary = Convert.ToDecimal(reader["Salary"])
            });
        }

        return list;
    }

    public async Task<Employee?> GetByIdAsync(int id)
    {
        using var conn = new MySqlConnection(_connectionString);
        using var cmd = new MySqlCommand("SELECT * FROM Employees WHERE Id = @id", conn);

        cmd.Parameters.AddWithValue("@id", id);

        await conn.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();

        if (await reader.ReadAsync())
        {
            return new Employee
            {
                Id = Convert.ToInt32(reader["Id"]),
                Name = reader["Name"].ToString() ?? string.Empty,
                Email = reader["Email"].ToString() ?? string.Empty,
                Salary = Convert.ToDecimal(reader["Salary"])
            };
        }

        return null;
    }

    public async Task CreateAsync(Employee emp)
    {
        using var conn = new MySqlConnection(_connectionString);

        string sql = @"INSERT INTO Employees (Name, Email, Salary)
                       VALUES (@name, @email, @salary)";

        using var cmd = new MySqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@name", emp.Name);
        cmd.Parameters.AddWithValue("@email", emp.Email);
        cmd.Parameters.AddWithValue("@salary", emp.Salary);

        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task UpdateAsync(int id, Employee emp)
    {
        using var conn = new MySqlConnection(_connectionString);

        string sql = @"UPDATE Employees
                       SET Name = @name, Email = @email, Salary = @salary
                       WHERE Id = @id";

        using var cmd = new MySqlCommand(sql, conn);

        cmd.Parameters.AddWithValue("@name", emp.Name);
        cmd.Parameters.AddWithValue("@email", emp.Email);
        cmd.Parameters.AddWithValue("@salary", emp.Salary);
        cmd.Parameters.AddWithValue("@id", id);

        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task DeleteAsync(int id)
    {
        using var conn = new MySqlConnection(_connectionString);

        string sql = "DELETE FROM Employees WHERE Id = @id";

        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@id", id);

        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }
}
