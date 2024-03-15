using System.Reflection;
using FluentMigrator.Runner;
using IntegrationTests.infrastructure.persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TestContainers.Container.Abstractions.Hosting;
using TestContainers.Container.Database.AdoNet;
using TestContainers.Container.Database.Hosting;
using TestContainers.Container.Database.PostgreSql;

namespace Test;

public class PostgresRepositorySetUp : IDisposable
{
    private readonly PostgreSqlContainer _postgreSqlContainer = BuildPostgreSqlContainer();
    public IServiceProvider? ServiceProvider { get; private set; }

    public PostgresRepositorySetUp()
    {
        InitializeAsync().Wait();
    }

    private async Task InitializeAsync()
    {
        await _postgreSqlContainer.StartAsync();

        var services = new ServiceCollection();
        SetUpRepositoriesDependencyInjection(services);
        SetUpDatabaseMigration(services);
    }
    
    public void Dispose()
    {
        CleanupAsync().Wait();
    }

    private async Task CleanupAsync()
    {
        await _postgreSqlContainer.StopAsync();
    }

    private void SetUpRepositoriesDependencyInjection(IServiceCollection services)
    {
        services.AddSingleton<IConfiguration>(BuildConnectionConfiguration(_postgreSqlContainer));
        services.AddSingleton<IDbConnectionFactory, NpgsqlConnectionFactory>();
        services.AddScoped<IClientRepository, ClientRepository>();
    }

    private void SetUpDatabaseMigration(IServiceCollection services)
    {
        services.AddFluentMigratorCore()
            .ConfigureRunner(runner => runner
                .AddPostgres()
                .WithGlobalConnectionString(_postgreSqlContainer.GetConnectionString())
                .ScanIn(Assembly.Load("IntegrationTests")).For.Migrations());
        ServiceProvider = services.BuildServiceProvider();

        using var scope = ServiceProvider.CreateScope();
        {
            var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
            runner.MigrateUp();
        }
    }

    private static IConfigurationRoot BuildConnectionConfiguration(AdoNetContainer sqlContainer)
    {
        return new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                { "ConnectionStrings:DefaultConnection", sqlContainer.GetConnectionString() }
            }!)
            .Build();
    }
    
    private static PostgreSqlContainer BuildPostgreSqlContainer()
    {
        return new ContainerBuilder<PostgreSqlContainer>()
            .ConfigureDatabaseConfiguration("test_db", "test_user", "test_password")
            .Build();
    }
}