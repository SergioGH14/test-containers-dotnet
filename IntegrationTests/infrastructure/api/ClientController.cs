using IntegrationTests.domain;
using IntegrationTests.infrastructure.persistence;
using Microsoft.AspNetCore.Mvc;

namespace IntegrationTests.infrastructure.api;


[ApiController]
[Route("clients")]
public class ClientController(IClientRepository repository) : ControllerBase
{
    
    [HttpPost(Name = "AddClient")]
    public void AddClient(Client client)
    {
        repository.AddClientAsync(client);
    }
    
}