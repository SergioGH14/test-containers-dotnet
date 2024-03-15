using IntegrationTests.domain;

namespace IntegrationTests.infrastructure.persistence;

public interface IClientRepository
{
    Task<IEnumerable<Client>> GetAllClientsAsync();
    Task AddClientAsync(Client client);
}