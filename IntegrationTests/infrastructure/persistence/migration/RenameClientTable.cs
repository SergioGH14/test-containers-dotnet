using FluentMigrator;

namespace IntegrationTests.infrastructure.persistence.migration;

[Migration(2)]
public class RenameClientTableName : Migration
{
    public override void Up()
    {
        Rename.Table("Clients").To("clients");
    }

    public override void Down()
    {
        Rename.Table("clients").To("Clients");
    }
}