using MySql.Data.MySqlClient;

namespace Backend.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(string connectionString)
    {
        using var conn = new MySqlConnection(connectionString);
        await conn.OpenAsync();

        // 1. Existing App Tables
        await Execute(conn, """
            CREATE TABLE IF NOT EXISTS Inventories (
                Id          INT           NOT NULL AUTO_INCREMENT PRIMARY KEY,
                Name        VARCHAR(200)  NOT NULL,
                Description VARCHAR(500)  NULL,
                Quantity    INT           NOT NULL DEFAULT 0,
                Price       DECIMAL(18,2) NOT NULL DEFAULT 0
            );
            """);

        await Execute(conn, """
            CREATE TABLE IF NOT EXISTS Employees (
                Id     INT           NOT NULL AUTO_INCREMENT PRIMARY KEY,
                Name   VARCHAR(100)  NOT NULL,
                Email  VARCHAR(150)  NOT NULL,
                Salary DECIMAL(18,2) NOT NULL DEFAULT 0
            );
            """);

        await Execute(conn, """
            CREATE TABLE IF NOT EXISTS Roles (
                Id   INT          NOT NULL AUTO_INCREMENT PRIMARY KEY,
                Name VARCHAR(50)  NOT NULL UNIQUE
            );
            """);

        await Execute(conn, """
            CREATE TABLE IF NOT EXISTS Accounts (
                Id       INT          NOT NULL AUTO_INCREMENT PRIMARY KEY,
                Username VARCHAR(100) NOT NULL UNIQUE,
                Password VARCHAR(255) NOT NULL,
                Email    VARCHAR(150) NOT NULL,
                RoleId   INT          NOT NULL,
                FOREIGN KEY (RoleId) REFERENCES Roles(Id)
            );
            """);

        await Execute(conn, """
            INSERT IGNORE INTO Roles (Id, Name) VALUES
                (1, 'Admin'),
                (2, 'Manager'),
                (3, 'Employee');
            """);

        // 2. Execute new Sales & Inventory tables from SQL file
        var sqlFilePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory())!.FullName, "inventory_db.sql");
        if (File.Exists(sqlFilePath))
        {
            var sqlCommands = await File.ReadAllTextAsync(sqlFilePath);
            // MySqlConnector allows multiple statements by default in the same command if configured, or we just execute it directly
            using var cmd = new MySqlCommand(sqlCommands, conn);
            try
            {
                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error executing SQL script: {ex.Message}");
            }
        }
    }

    private static async Task Execute(MySqlConnection conn, string sql)
    {
        using var cmd = new MySqlCommand(sql, conn);
        await cmd.ExecuteNonQueryAsync();
    }
}
