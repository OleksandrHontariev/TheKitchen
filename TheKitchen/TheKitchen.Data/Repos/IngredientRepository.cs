using System;
using System.Collections.Generic;
using System.Data;
using TheKitchen.Data.Abstractions;
using TheKitchen.Data.Entities;
using TheKitchen.Data.Infrastructure;
using Dapper;
using NLog;
using System.Linq;
using System.Diagnostics;

namespace TheKitchen.Data.Repos
{
    public class IngredientRepository : IIngredientRepository
    {
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();
        IDbConnection _connection;
        public IngredientRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public IEnumerable<Ingredient> GetByRecipeId(int id)
        {
            string sql = @"
                        SELECT
                            i.Id,
                            i.KitchenId,
                            i.CategoryId,
                            i.Title,
                            i.BaseUnit,
                            i.StockQuantity
                        FROM RecipeDetails rd
                        JOIN Ingredients i ON rd.IngredientId = i.Id
                        WHERE RecipeId = @Id
                        ORDER BY rd.Ordering
                        ";

            try
            {
                IEnumerable<Ingredient> data = _connection.Query<Ingredient>(sql, new { Id = id });
                Logger.Info($"Getting list of ingredients of length {data.Count()}");
                return data;
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error while getting list of ingredients");
                throw;
            }
        }

        public Ingredient GetById(int id)
        {
            string sql = "SELECT * FROM Ingredients WHERE Id = @Id";
            Ingredient ingredient = null;
            try
            {
                ingredient = _connection.QueryFirstOrDefault<Ingredient>(sql, new { Id = id });
                if (ingredient == null)
                {
                    Logger.Info($"Ingredient with Id={id} not found.");
                } else
                {
                    Logger.Info($"Retrieved ingredient: {ingredient}");
                }
                return ingredient;
            } catch(Exception ex)
            {
                Logger.Error(ex, $"Error fetching ingredient Id={id}");
                throw;
            }
        }

        public PagedResult<Ingredient> GetPagedBySearch(int kitchenId, int? categoryId, string query, int page, int pageSize)
        {
            string pagedBySearchSql = @"
                                        SELECT *
                                        FROM Ingredients
                                        WHERE 
                                            KitchenId = @KitchenId
                                            AND (
                                                    (@CategoryId IS NULL AND CategoryId IS NULL)
                                                 OR (@CategoryId IS NOT NULL AND CategoryId = @CategoryId)
                                                )
                                            AND (@Query IS NULL OR Title LIKE '%' + @Query + '%')
                                        ORDER BY Title
                                        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
                                    ";

            string totalCountSql = @"
                                    SELECT COUNT(*)
                                    FROM Ingredients
                                    WHERE 
                                        KitchenId = @KitchenId
                                        AND (
                                                (@CategoryId IS NULL AND CategoryId IS NULL)
                                             OR (@CategoryId IS NOT NULL AND CategoryId = @CategoryId)
                                            )
                                        AND (@Query IS NULL OR Title LIKE '%' + @Query + '%');
                                ";

            try
            {
                int offset = (page - 1) * pageSize;

                var parameters = new
                {
                    KitchenId = kitchenId,
                    CategoryId = categoryId,
                    Query = string.IsNullOrWhiteSpace(query) ? null : query,
                    Offset = offset,
                    PageSize = pageSize
                };

                IEnumerable<Ingredient> items = _connection.Query<Ingredient>(pagedBySearchSql, parameters);
                int totalCount = _connection.ExecuteScalar<int>(totalCountSql, parameters);

                Logger.Info($"Paged search: items={items.Count()}, total={totalCount}, page={page}");

                return new PagedResult<Ingredient>
                {
                    Items = items.ToList(),
                    TotalCount = totalCount,
                    PageNumber = page,
                    PageSize = pageSize
                };
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Error while paginating ingredients");
                throw;
            }
        }

        public int Add(Ingredient ingredient)
        {
            string sql = @"INSERT INTO Ingredients
                        (KitchenId, CategoryId, Title, BaseUnit, StockQuantity)
                        VALUES
                        (@KitchenId, @CategoryId, @Title, @BaseUnit, @StockQuantity)
                        SELECT CAST(SCOPE_IDENTITY() AS INT)";

            try
            {
                int id = _connection.ExecuteScalar<int>(sql, ingredient);
                Logger.Info($"Added ingredient: {ingredient.Title} with Id={id}");
                return id;
            } catch(Exception ex)
            {
                Logger.Error(ex, $"Error adding ingredient: {ingredient.Title}");
                throw;
            }
        }

        public bool Update(Ingredient ingredient)
        {
            string sql = @"
                        UPDATE Ingredients
                        SET KitchenId = @KitchenId,
                            CategoryId = @CategoryId,
                            Title = @Title,
                            BaseUnit = @BaseUnit,
                            StockQuantity = @StockQuantity
                        WHERE Id = @Id";

            try
            {
                int affectedRows = _connection.Execute(sql, ingredient);
                Logger.Info($"Updated ingredient Id={ingredient.Id}. Rows affected: {affectedRows}");
                return affectedRows == 1;
            } catch (Exception ex)
            {
                Logger.Error(ex, $"Error updating ingredient Id={ingredient.Id}");
                throw;
            }
        }

        public bool Delete(int id)
        {
            using (var tran = _connection.BeginTransaction())
            {
                try
                {
                    string deleteDetailsSql = "DELETE FROM RecipeDetails WHERE IngredientId = @Id";
                    _connection.Execute(deleteDetailsSql, new { Id = id }, tran);

                    string deleteIngredientSql = "DELETE FROM Ingredients WHERE Id = @Id";
                    int affectedRows = _connection.Execute(deleteIngredientSql, new { Id = id }, tran);

                    tran.Commit();

                    Logger.Info($"Deleted ingredient Id={id}, rows affected: {affectedRows}");
                    return affectedRows == 1;
                } catch (Exception ex)
                {
                    tran.Rollback();
                    Logger.Error(ex, $"Error deleting ingredient Id={id}");
                    throw;
                }
            }
        }
    }
}
