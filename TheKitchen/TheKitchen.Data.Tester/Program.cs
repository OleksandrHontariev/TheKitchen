using Microsoft.Data.SqlClient;
using System;
using System.Data;
using TheKitchen.Data;
using System.IO;
using TheKitchen.Data.Abstractions;
using TheKitchen.Data.Repos;
using TheKitchen.Data.Entities;
using System.Collections.Generic;

string dbName = "TheKitchen";
// CreateDatabase(dbName);
CRUDL_Kitchen();

Console.ReadLine();

static void CreateDatabase(string dbName)
{
    string masterCS = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
    using (IDbConnection connection = new SqlConnection(masterCS))
    {
        connection.Open();
        DatabaseCreator.CreateDatabase(connection, dbName);
    }

    string targetCS = @$"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog={dbName};Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
    using (IDbConnection connection_2 = new SqlConnection(targetCS))
    {
        connection_2.Open();
        string scriptsFolter = Path.Combine(AppContext.BaseDirectory, "Sql", "Tables");
        DatabaseCreator.CreateDatabaseTables(connection_2, scriptsFolter);
    }
}

static void CRUDL_Kitchen()
{
    string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=TheKitchen;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
    IDbConnection connection = new SqlConnection(connectionString);
    IKitchenRepository repo = new KitchenRepository(connection);

    int mcId = repo.Add(new Kitchen { Name = "McDonald's", Description = "Имитация ресторана Макдональдц", TablesCount = 80 });
    int mushlaId = repo.Add(new Kitchen { Name = "Mushla", Description = "Имитация ресторана Борисова Мушля", TablesCount = 55 });
    int pusataHataId = repo.Add(new Kitchen { Name = "Пузата Хата", Description = "Имитация ресторана Пузата Хата", TablesCount = 67 });

    Kitchen mcDonaldc = repo.GetById(mcId);
    mcDonaldc.Description = "MaaakDonald's";
    repo.Update(mcDonaldc);

    IEnumerable<Kitchen> all = repo.GetAll();
    repo.Delete(mcId);
    repo.Delete(mcId);
}

