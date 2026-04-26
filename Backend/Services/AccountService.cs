using MySql.Data.MySqlClient;
using Backend.Models;

namespace Backend.Services;

public class AccountService : IAccountService
{
    private readonly string _connectionString;

    public AccountService(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("MySqlDb")
            ?? throw new InvalidOperationException("Connection string 'MySqlDb' not found.");
    }

    public async Task<List<Account>> GetAllAsync()
    {
        var list = new List<Account>();
        using var conn = new MySqlConnection(_connectionString);
        const string sql = """
            SELECT a.Id, a.Username, a.Password, a.Email, a.RoleId, r.Name AS RoleName
            FROM Accounts a
            LEFT JOIN Roles r ON a.RoleId = r.Id
            """;
        using var cmd = new MySqlCommand(sql, conn);
        await conn.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            list.Add(new Account
            {
                Id       = Convert.ToInt32(reader["Id"]),
                Username = reader["Username"].ToString()!,
                Password = reader["Password"].ToString()!,
                Email    = reader["Email"].ToString()!,
                RoleId   = Convert.ToInt32(reader["RoleId"]),
                RoleName = reader["RoleName"] == DBNull.Value ? null : reader["RoleName"].ToString()
            });
        }
        return list;
    }

    public async Task<Account?> GetByIdAsync(int id)
    {
        using var conn = new MySqlConnection(_connectionString);
        const string sql = """
            SELECT a.Id, a.Username, a.Password, a.Email, a.RoleId, r.Name AS RoleName
            FROM Accounts a
            LEFT JOIN Roles r ON a.RoleId = r.Id
            WHERE a.Id = @id
            """;
        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@id", id);
        await conn.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Account
            {
                Id       = Convert.ToInt32(reader["Id"]),
                Username = reader["Username"].ToString()!,
                Password = reader["Password"].ToString()!,
                Email    = reader["Email"].ToString()!,
                RoleId   = Convert.ToInt32(reader["RoleId"]),
                RoleName = reader["RoleName"] == DBNull.Value ? null : reader["RoleName"].ToString()
            };
        }
        return null;
    }

    public async Task<Account?> GetByUsernameAsync(string username)
    {
        using var conn = new MySqlConnection(_connectionString);
        const string sql = """
            SELECT a.Id, a.Username, a.Password, a.Email, a.RoleId, r.Name AS RoleName
            FROM Accounts a
            LEFT JOIN Roles r ON a.RoleId = r.Id
            WHERE a.Username = @username
            """;
        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@username", username);
        await conn.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Account
            {
                Id       = Convert.ToInt32(reader["Id"]),
                Username = reader["Username"].ToString()!,
                Password = reader["Password"].ToString()!,
                Email    = reader["Email"].ToString()!,
                RoleId   = Convert.ToInt32(reader["RoleId"]),
                RoleName = reader["RoleName"] == DBNull.Value ? null : reader["RoleName"].ToString()
            };
        }
        return null;
    }

    public async Task<Account?> AuthenticateAsync(string username, string password)
    {
        using var conn = new MySqlConnection(_connectionString);
        const string sql = """
            SELECT a.Id, a.Username, a.Password, a.Email, a.RoleId, r.Name AS RoleName
            FROM Accounts a
            LEFT JOIN Roles r ON a.RoleId = r.Id
            WHERE a.Username = @username AND a.Password = @password
            """;
        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@username", username);
        cmd.Parameters.AddWithValue("@password", password);
        await conn.OpenAsync();
        using var reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new Account
            {
                Id       = Convert.ToInt32(reader["Id"]),
                Username = reader["Username"].ToString()!,
                Password = reader["Password"].ToString()!,
                Email    = reader["Email"].ToString()!,
                RoleId   = Convert.ToInt32(reader["RoleId"]),
                RoleName = reader["RoleName"] == DBNull.Value ? null : reader["RoleName"].ToString()
            };
        }
        return null;
    }

    public async Task CreateAsync(Account account)
    {
        using var conn = new MySqlConnection(_connectionString);
        const string sql = """
            INSERT INTO Accounts (Username, Password, Email, RoleId)
            VALUES (@username, @password, @email, @roleId)
            """;
        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@username", account.Username);
        cmd.Parameters.AddWithValue("@password", account.Password);
        cmd.Parameters.AddWithValue("@email",    account.Email);
        cmd.Parameters.AddWithValue("@roleId",   account.RoleId);
        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task UpdateAsync(int id, Account account)
    {
        using var conn = new MySqlConnection(_connectionString);
        const string sql = """
            UPDATE Accounts
            SET Username = @username, Password = @password, Email = @email, RoleId = @roleId
            WHERE Id = @id
            """;
        using var cmd = new MySqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@username", account.Username);
        cmd.Parameters.AddWithValue("@password", account.Password);
        cmd.Parameters.AddWithValue("@email",    account.Email);
        cmd.Parameters.AddWithValue("@roleId",   account.RoleId);
        cmd.Parameters.AddWithValue("@id",       id);
        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }

    public async Task DeleteAsync(int id)
    {
        using var conn = new MySqlConnection(_connectionString);
        using var cmd  = new MySqlCommand("DELETE FROM Accounts WHERE Id = @id", conn);
        cmd.Parameters.AddWithValue("@id", id);
        await conn.OpenAsync();
        await cmd.ExecuteNonQueryAsync();
    }
}
