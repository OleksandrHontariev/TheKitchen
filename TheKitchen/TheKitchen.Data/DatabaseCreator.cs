using NLog;
using System;
using System.Data;
using System.IO;
using Dapper;

namespace TheKitchen.Data
{
    public static class DatabaseCreator
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public static void CreateDatabase(IDbConnection dbConnection, string dbName)
        {
            string sql = @$"
                        CREATE DATABASE {dbName}
                        COLLATE Cyrillic_General_CI_AS
                        ";
            try
            {
                int rowsAffected = dbConnection.Execute(sql);
                Logger.Info($"Database {dbName} successfully created, rows affected: {rowsAffected}");
            } catch (Exception ex)
            {
                Logger.Error(ex, $"Cannot create database with name {dbName}");
            }
        }
        public static void CreateDatabaseTables (IDbConnection dbConnection, string sqlFoterPath)
        {
            string[] files = {
                "Kitchens.sql",
                "Recipes.sql",
                "Units.sql",
                "Ingredients.sql",
                "IngredientPrices.sql",
                "UnitConversions.sql",
                "RecipeDetails.sql",
                "Chefs.sql",
                "ChefDetails.sql",
                "Dishes.sql",
                "MenuItems.sql",
                "MenuItemDetails.sql"
            };
            
            foreach(string fileName in files)
            {
                string filePath = Path.Combine(sqlFoterPath, fileName);
                if (!File.Exists(filePath))
                {
                    Logger.Error($"File {Path.GetFileName(filePath)} with path: {filePath} not exists");
                } else
                {
                    string fileContent = string.Empty;
                    try
                    {
                        fileContent = File.ReadAllText(filePath);
                    } catch
                    {
                        Logger.Error($"Cannot read file {Path.GetFileName(filePath)} with path: {filePath}");
                        continue;
                    }

                    try
                    {
                        int affectedRows = dbConnection.Execute(fileContent);
                        Logger.Info($"Script {Path.GetFileName(filePath)} executed successfully. Affected rows: {affectedRows}");
                    } catch (Exception ex)
                    {
                        Logger.Error(ex, $"Error while executing script {Path.GetFileName(filePath)}");
                    }
                }
            }
            Logger.Info("All scripts executed!");
        }
    }
}
