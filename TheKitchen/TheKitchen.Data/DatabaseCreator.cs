using NLog;
using System;
using System.Data;
using System.IO;
using Dapper;
using System.Linq;
using System.Collections.Generic;

namespace TheKitchen.Data
{
    public static class DatabaseCreator
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static string KitchenIdPlaceholder = "{{kitchenId}}";

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
                "IngredientCategories.sql",
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

        public static void SeedStaticData (IDbConnection dbConnection, int kitchenId, string seedFolderPath)
        {
            if (!Directory.Exists(seedFolderPath))
            {
                Logger.Error($"Seed folder not found: {seedFolderPath}");
                return;
            }

            IEnumerable<string> sqlFiles = Directory.GetFiles(seedFolderPath, "*.sql").OrderBy(p => p).ToList();
            foreach (var filePath in sqlFiles)
            {
                try
                {
                    string sql = File.ReadAllText(filePath);

                    if (string.IsNullOrWhiteSpace(sql))
                    {
                        Logger.Warn($"File {Path.GetFileName(filePath)} is empty, skipping.");
                        continue;
                    }

                    sql = sql.Replace(KitchenIdPlaceholder, kitchenId.ToString());
                    int affectedRows = dbConnection.Execute(sql);
                    Logger.Info($"Seed file {Path.GetFileName(filePath)} executed successfully. Rows affected: {affectedRows}");
                } catch(Exception ex)
                {
                    Logger.Error(ex, $"Error executing seed file: {Path.GetFileName(filePath)}");
                }
            }
            Logger.Info("All seed files executed!");
        }
    }
}
