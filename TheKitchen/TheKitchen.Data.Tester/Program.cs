using Microsoft.Data.SqlClient;
using System;
using System.Data;
using TheKitchen.Data;
using System.IO;

string dbName = "TheKitchen2";
CreateDatabase(dbName);
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

