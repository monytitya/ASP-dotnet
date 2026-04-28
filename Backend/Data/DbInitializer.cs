using MySql.Data.MySqlClient;

namespace Backend.Data;

public static class DbInitializer
{
    public static async Task InitializeAsync(string connectionString)
    {
        using var conn = new MySqlConnection(connectionString);
        await conn.OpenAsync();

        await Execute(conn, """
            CREATE TABLE IF NOT EXISTS Inventories (
                id          INT           NOT NULL AUTO_INCREMENT PRIMARY KEY,
                name        VARCHAR(200)  NOT NULL,
                description VARCHAR(500)  NULL,
                quantity    INT           NOT NULL DEFAULT 0,
                price       DECIMAL(18,2) NOT NULL DEFAULT 0
            );
            """);

        await Execute(conn, """
            CREATE TABLE IF NOT EXISTS Employees (
                id        INT           NOT NULL AUTO_INCREMENT PRIMARY KEY,
                name      VARCHAR(100)  NOT NULL,
                email     VARCHAR(150)  NOT NULL,
                salary    DECIMAL(18,2) NOT NULL DEFAULT 0,
                image_url VARCHAR(500)  NULL
            );
            """);

        try { await Execute(conn, "ALTER TABLE Employees ADD COLUMN image_url VARCHAR(500) NULL;"); } catch { }
        try { await Execute(conn, "ALTER TABLE Inventories ADD COLUMN description VARCHAR(500) NULL;"); } catch { }
        try { await Execute(conn, "ALTER TABLE Products ADD COLUMN image_url VARCHAR(500) NULL;"); } catch { }

        await Execute(conn, """
            CREATE TABLE IF NOT EXISTS roles (
                id   INT          NOT NULL AUTO_INCREMENT PRIMARY KEY,
                name VARCHAR(50)  NOT NULL UNIQUE
            );
            """);

        await Execute(conn, """
            CREATE TABLE IF NOT EXISTS accounts (
                id       INT          NOT NULL AUTO_INCREMENT PRIMARY KEY,
                username VARCHAR(100) NOT NULL UNIQUE,
                password VARCHAR(255) NOT NULL,
                email    VARCHAR(150) NOT NULL,
                role_id  INT          NOT NULL,
                FOREIGN KEY (role_id) REFERENCES roles(id)
            );
            """);

        await Execute(conn, """
            INSERT IGNORE INTO Roles (Id, Name) VALUES
                (1, 'Admin'),
                (2, 'Manager'),
                (3, 'Employee');
            """);

        var sqlFilePath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory())!.FullName, "inventory_db.sql");
        if (File.Exists(sqlFilePath))
        {
            var sqlText = await File.ReadAllTextAsync(sqlFilePath);
            // Simple split by semicolon. Note: This might fail if semicolons exist within strings.
            // But for this simple schema file it should be fine.
            var sqlCommands = sqlText.Split(';', StringSplitOptions.RemoveEmptyEntries);
            
            foreach (var sql in sqlCommands)
            {
                var trimmedSql = sql.Trim();
                if (string.IsNullOrEmpty(trimmedSql)) continue;

                try
                {
                    await Execute(conn, trimmedSql);
                }
                catch (MySqlException ex) when (ex.Number == 1061) // Duplicate key name
                {
                    // Ignore duplicate index errors
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error executing SQL command: {ex.Message}");
                    Console.WriteLine($"Command: {trimmedSql}");
                }
            }
        }
    }

    private static async Task Execute(MySqlConnection conn, string sql)
    {
        using var cmd = new MySqlCommand(sql, conn);
        await cmd.ExecuteNonQueryAsync();
    }
}
