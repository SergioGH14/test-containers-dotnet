using FluentMigrator;

namespace IntegrationTests.infrastructure.persistence.migration;

[Migration(1)]
public class AddClientTable : Migration
{
    public override void Up()
    {
        Create.Table("Clients")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("name").AsString()
            .WithColumn("surname").AsString()
            .WithColumn("created_at").AsDateTime()
            .WithColumn("updated_at").AsDateTime();
    }

    public override void Down()
    {
        Delete.Table("Clients");
    }
}