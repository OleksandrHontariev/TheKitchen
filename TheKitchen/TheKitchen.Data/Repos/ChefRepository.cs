using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using TheKitchen.Data.Abstractions;
using TheKitchen.Data.Entities;
using NLog;
using Dapper;
using System.IO;

namespace TheKitchen.Data.Repos
{
    public class ChefRepository : IChefRepository
    {
        IDbConnection _connection;

        private readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public ChefRepository(IDbConnection connection)
        {
            _connection = connection;
        }
        public IEnumerable<Chef> SearchChefs(string query)
        {
            string sql = @"SELECT
                                ch.Id,
                                ch.KitchenId,
                                ch.FName,
                                ch.MName,
                                ch.LName,
                                ch.HideDate,
                                ch.ExpirienceYears,

                                cd.PhoneNumber,
                                cd.PasspordNumber,
                                cd.BirthDate,
                                cd.Country,
                                cd.Region,
                                cd.City,
                                cd.Street,
                                cd.House,
                                cd.Apartment,
                                cd.PostalCode,
                                cd.Notes
                        FROM Chefs ch
                        LEFT JOIN
                            ChefDetails cd ON ch.Id = cd.ChefId
                        WHERE (@Query IS NULL OR @Query = '' OR ch.LName LIKE @QueryPattern)";

            try
            {
                IEnumerable<Chef> chefs = _connection.Query<Chef, ChefDetail, Chef>(sql, (chef, details) => {
                    chef.ChefDetail = details;
                    return chef;
                }, new { Query = query, QueryPattern = string.IsNullOrWhiteSpace(query) ? "%" : query + '%' },
                splitOn: "PhoneNumber");

                Logger.Info($"Getting list of chefs of length {chefs.Count()} by query={query}");
                return chefs;

            } catch (Exception ex)
            {
                Logger.Error(ex, $"Error while getting list of chefs");
                throw;
            }
        }
        public Chef GetById(int id)
        {
            string sql = @"SELECT
                                ch.Id,
                                ch.KitchenId,
                                ch.FName,
                                ch.MName,
                                ch.LName,
                                ch.HideDate,
                                ch.ExpirienceYears,

                                cd.PhoneNumber,
                                cd.PasspordNumber,
                                cd.BirthDate,
                                cd.Country,
                                cd.Region,
                                cd.City,
                                cd.Street,
                                cd.House,
                                cd.Apartment,
                                cd.PostalCode,
                                cd.Notes
                            FROM Chefs ch
                            LEFT JOIN ChefDetails cd
                                ON ch.Id = cd.ChefId
                            WHERE ch.Id = @Id";

            try
            {
                Chef chef = _connection.Query<Chef, ChefDetail, Chef>(
                    sql,
                    (chef, detail) => {
                        chef.ChefDetail = detail;
                        return chef;
                    },
                    new { Id = id },
                    splitOn: "PhoneNumber").FirstOrDefault();

                if (chef != null)
                {
                    Logger.Info($"Getting chef with data: {chef.ToString()}");
                    return chef;
                } else
                {
                    Logger.Info($"The chef with id {id} not exists.");
                    return null;
                }
                
            } catch (Exception ex)
            {
                Logger.Error(ex, $"Error while getting chef by id {id}");
                throw;
            }
        }
        public int Add(Chef chef)
        {
            string addChefSql = @"INSERT INTO Chefs
                                        (KitchenId,
                                         FName,
                                         MName,
                                         LName,
                                         HideDate,
                                         ExpirienceYears)
                                    VALUES (@KitchenId,
                                            @FName,
                                            @MName,
                                            @LName,
                                            @HideDate,
                                            @ExpirienceYears)
                                    SELECT CAST(SCOPE_IDENTITY() AS INT)";

            string addChefDetailSql = @"INSERT INTO ChefDetails
                                            (KitchenId,
                                             ChefId,
                                             PhoneNumber,
                                             PasspordNumber,
                                             BirthDate,
                                             Country,
                                             Region,
                                             City,
                                             Street,
                                             House,
                                             Apartment,
                                             PostalCode,
                                             Notes)
                                        VALUES (
                                             @KitchenId,
                                             @ChefId,
                                             @PhoneNumber,
                                             @PasspordNumber,
                                             @BirthDate,
                                             @Country,
                                             @Region,
                                             @City,
                                             @Street,
                                             @House,
                                             @Apartment,
                                             @PostalCode,
                                             @Notes
                                        )";

            try
            {
                using var transaction = _connection.BeginTransaction();
                int id = _connection.QuerySingle<int>(addChefSql, chef, transaction: transaction);

                if (chef.ChefDetail != null)
                {
                    var addChefDetailsParameters = new
                    {
                        chef.KitchenId,
                        ChefId = id,
                        chef.ChefDetail.PhoneNumber,
                        chef.ChefDetail.PasspordNumber,
                        chef.ChefDetail.BirthDate,
                        chef.ChefDetail.Country,
                        chef.ChefDetail.Region,
                        chef.ChefDetail.City,
                        chef.ChefDetail.Street,
                        chef.ChefDetail.House,
                        chef.ChefDetail.Apartment,
                        chef.ChefDetail.PostalCode,
                        chef.ChefDetail.Notes
                    };

                    _connection.Execute(addChefDetailSql, addChefDetailsParameters, transaction: transaction);
                }
                transaction.Commit();

                Logger.Info($"Added chef with id={id}");
                return id;

            } catch (Exception ex)
            {
                Logger.Error(ex, $"Error adding chef");
                throw;
            }
        }
        public bool Update(Chef chef)
        {
            string updateChefDetailSql = @"UPDATE ChefDetails
                                            SET
                                                KitchenId = @KitchenId,
                                                ChefId = @ChefId,
                                                PhoneNumber = @PhoneNumber,
                                                PasspordNumber = @PasspordNumber,
                                                BirthDate = @BirthDate,
                                                Country = @Country,
                                                Region = @Region,
                                                City = @City,
                                                Street = @Street,
                                                House = @House,
                                                Apartment = @Apartment,
                                                PostalCode = @PostalCode,
                                                Notes = @Notes
                                            WHERE ChefId = @ChefId";

            string updateChefSql = @"UPDATE Chefs
                                      SET
                                         KitchenId = @KitchenId,
                                         FName = @FName,
                                         MName = @MName,
                                         LName = @LName,
                                         HideDate = @HideDate,
                                         ExpirienceYears = @ExpirienceYears
                                       WHERE Id = @Id";

            try
            {
                using var transaction = _connection.BeginTransaction();
                int updateChefDetailRowsAffected = 0;
                if (chef.ChefDetail != null)
                {
                    updateChefDetailRowsAffected = _connection.Execute(updateChefDetailSql, new
                    {
                        chef.KitchenId,
                        ChefId = chef.Id,
                        chef.ChefDetail.PhoneNumber,
                        chef.ChefDetail.PasspordNumber,
                        chef.ChefDetail.BirthDate,
                        chef.ChefDetail.Country,
                        chef.ChefDetail.Region,
                        chef.ChefDetail.City,
                        chef.ChefDetail.Street,
                        chef.ChefDetail.House,
                        chef.ChefDetail.Apartment,
                        chef.ChefDetail.PostalCode,
                        chef.ChefDetail.Notes
                    }, transaction: transaction);
                }

                int updateChefRowsAffected = _connection.Execute(updateChefSql, chef, transaction: transaction);
                transaction.Commit();
                Logger.Info($"Updated chef detail ChefId={chef.Id}. Rows affected: {updateChefDetailRowsAffected}");
                Logger.Info($"Updated chef Id={chef.Id}. Rows affected: {updateChefRowsAffected}");
                return updateChefRowsAffected == 1;
            } catch (Exception ex)
            {
                Logger.Error(ex, $"Error updating chef Id={chef.Id}");
                throw;
            }
        }

        public bool Delete(int id)
        {
            string deleteChefDetailSql = "DELETE FROM ChefDetails WHERE ChefId = @Id";
            string deleteChefSql = "DELETE FROM Chefs WHERE Id = @Id";

            try
            {
                using var transaction = _connection.BeginTransaction();

                int deleteChefDetailsAffectedRows = _connection.Execute(deleteChefDetailSql,
                                                                        new { Id = id },
                                                                        transaction: transaction);

                int deleteChefAffectedRows = _connection.Execute(deleteChefSql,
                                                                 new { Id = id },
                                                                 transaction: transaction);
                transaction.Commit();

                Logger.Info($"Deleted chef Id={id}. Rows affected: {deleteChefAffectedRows}");
                Logger.Info($"Deleted chefDetail ChefId={id}. Rows affected: {deleteChefDetailsAffectedRows}");

                return deleteChefAffectedRows == 1;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, $"Error deleting chef and chefDetail Id={id}");
                throw;
            }
        }

    }
}
