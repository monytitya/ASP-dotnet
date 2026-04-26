using MySql.Data.MySqlClient;

namespace Backend.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(string connectionString)
    {
        using var conn = new MySqlConnection(connectionString);
        await conn.OpenAsync();

        // Employees table
        await Execute(conn, """
            CREATE TABLE IF NOT EXISTS Employees (
                Id     INT           NOT NULL AUTO_INCREMENT PRIMARY KEY,
                Name   VARCHAR(100)  NOT NULL,
                Email  VARCHAR(150)  NOT NULL,
                Salary DECIMAL(18,2) NOT NULL DEFAULT 0
            );
            """);

        // Roles table
        await Execute(conn, """
            CREATE TABLE IF NOT EXISTS Roles (
                Id   INT          NOT NULL AUTO_INCREMENT PRIMARY KEY,
                Name VARCHAR(50)  NOT NULL UNIQUE
            );
            """);

        // Accounts table — references Roles
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

        // Seed default roles if empty
        await Execute(conn, """
            INSERT IGNORE INTO Roles (Id, Name) VALUES
                (1, 'Admin'),
                (2, 'Manager'),
                (3, 'Employee');
            """);
    }

    private static async Task Execute(MySqlConnection conn, string sql)
    {
        using var cmd = new MySqlCommand(sql, conn);
        await cmd.ExecuteNonQueryAsync();
    }
}
