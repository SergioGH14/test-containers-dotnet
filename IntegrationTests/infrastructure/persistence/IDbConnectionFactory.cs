using Npgsql;

namespace IntegrationTests.infrastructure.persistence;

public interface IDbConnectionFactory
{
    NpgsqlConnection CreateDbConnection();
}

public class NpgsqlConnectionFactory(IConfiguration configuration) : IDbConnectionFactory
{
    private readonly string? _connectionString = configuration.GetConnectionString("DefaultConnection");

    public NpgsqlConnection CreateDbConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }
}