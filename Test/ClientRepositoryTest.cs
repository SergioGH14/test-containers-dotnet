using IntegrationTests.domain;
using IntegrationTests.infrastructure.persistence;
using Microsoft.Extensions.DependencyInjection;

namespace Test;

[Collection("Database_collection")]
public class ClientRepositoryTest(PostgresRepositorySetUp fixture)
{
    private readonly IClientRepository? _clientRepository = fixture.ServiceProvider.GetService<IClientRepository>();

    [Fact]
    public async Task AddClientWorksProperly()
    {
        await AssertDatabaseIsEmpty();

        await AddNewClientInDatabase();

        await AssertDatabaseHasClients();
    }

    private async Task AddNewClientInDatabase()
    {
        var newClient = new Client("name", "surname", DateTime.Now, DateTime.Now);
        await _clientRepository.AddClientAsync(newClient);
    }

    private async Task AssertDatabaseHasClients()
    {
        var finalClients = await _clientRepository.GetAllClientsAsync();
        Assert.NotEmpty(finalClients);
    }

    private async Task AssertDatabaseIsEmpty()
    {
        var initialClients = await _clientRepository.GetAllClientsAsync();
        Assert.Empty(initialClients);
    }
}