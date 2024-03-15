using Dapper;
using IntegrationTests.domain;

namespace IntegrationTests.infrastructure.persistence;

public class ClientRepository(IDbConnectionFactory connectionFactory) : IClientRepository
{
    public async Task<IEnumerable<Client>> GetAllClientsAsync()
    {
        await using var connection = connectionFactory.CreateDbConnection();
        await connection.OpenAsync();
        const string query = "SELECT * FROM public.clients";

        var result = await connection.QueryAsync<dynamic>(query);
        var clients = result.Select(client => new Client(
            client.name, client.surname, client.created_at, client.updated_at
        )).ToList();

        return clients;    }

    public async Task AddClientAsync(Client client)
    {
        const string sql =
            "INSERT INTO public.clients (name, surname, created_at, updated_at) VALUES (@Name, @Surname, @CreatedAt, @UpdatedAt)";
        await using var connection = connectionFactory.CreateDbConnection();
        await connection.OpenAsync();
        await using var transaction = await connection.BeginTransactionAsync();
        try
        {
            await connection.ExecuteAsync(sql, client, transaction: transaction);
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}