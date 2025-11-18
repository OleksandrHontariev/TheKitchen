using System;
using System.Collections.Generic;
using System.Data;
using TheKitchen.Data.Abstractions;
using TheKitchen.Data.Entities;
using Dapper;
using NLog;
using System.Linq;

namespace TheKitchen.Data.Repos
{
    public class KitchenRepository : IKitchenRepository
    {
        IDbConnection _connection;

        private readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public KitchenRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public IEnumerable<Kitchen> GetAll()
        {
            string sql = "SELECT * FROM Kitchens";

            try
            {
                IEnumerable<Kitchen> data = _connection.Query<Kitchen>(sql);
                Logger.Info($"Getting list of kitchens of length {data.Count()}");
                return data;
            } catch(Exception ex)
            {
                Logger.Error(ex, "Error while getting list of kitchens");
                throw;
            }
        }

        public Kitchen GetById(int id)
        {
            string sql = $"SELECT * FROM Kitchens WHERE Id = @Id";

            try
            {
                Kitchen kitchen = _connection.QueryFirstOrDefault<Kitchen>(sql, new { Id = id });
                if (kitchen != null)
                {
                    Logger.Info($"Getting kitchen with data: {kitchen.ToString()}");
                    return kitchen;
                } else
                {
                    Logger.Info($"The kitchen with id {id} not exists.");
                    return null;
                }
            } catch(Exception ex)
            {
                Logger.Error(ex, $"Error while getting kitchen by id {id}");
                throw;
            }
        }

        public int Add(Kitchen kitchen)
        {
            string sql = @"
                INSERT INTO Kitchens (Name, Description, TablesCount)
                VALUES (@Name, @Description, @TablesCount);
                SELECT CAST(SCOPE_IDENTITY() AS INT);
            ";

            try
            {
                int id = _connection.ExecuteScalar<int>(sql, kitchen);
                Logger.Info($"Added kitchen: {kitchen.Name} with Id={id}");
                return id;
            } catch(Exception ex)
            {
                Logger.Error(ex, $"Error adding kitchen: {kitchen.Name}");
                throw;
            }
        }

        public bool Update(Kitchen kitchen)
        {
            string sql = @"
                UPDATE Kitchens
                SET Name = @Name,
                    Description = @Description,
                    TablesCount = @TablesCount
                WHERE Id = @Id";

            try
            {
                int affectedRows = _connection.Execute(sql, kitchen);
                Logger.Info($"Updated kitchen Id={kitchen.Id}. Rows affected: {affectedRows}");
                return affectedRows == 1;
            } catch(Exception ex)
            {
                Logger.Error(ex, $"Error updating kitchen Id={kitchen.Id}");
                throw;
            }
        }

        public bool Delete (int id)
        {
            string sql = "DELETE FROM Kitchens WHERE Id = @Id";
            try
            {
                int affectedRows = _connection.Execute(sql, new { Id = id });
                Logger.Info($"Deleted kitchen Id={id}. Rows affected: {affectedRows}");
                return affectedRows == 1;
            } catch (Exception ex)
            {
                Logger.Error(ex, $"Error deleting kitchen Id={id}");
                throw;
            }
        }
    }
}
