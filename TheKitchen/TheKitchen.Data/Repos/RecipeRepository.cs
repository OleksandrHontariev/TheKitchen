    using Dapper;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TheKitchen.Data.Abstractions;
using TheKitchen.Data.Entities;
using TheKitchen.Data.Infrastructure;

namespace TheKitchen.Data.Repos
{
    public class RecipeRepository : IRecipeRepository
    {
        IDbConnection _connection;

        private readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public RecipeRepository(IDbConnection connection)
        {
            _connection = connection;
        }

        public Recipe GetById(int id)
        {
            string sql = "SELECT * FROM Recipes WHERE Id = @Id";
            try
            {
                Recipe recipe = _connection.QueryFirstOrDefault<Recipe>(sql, new { Id = id });
                if (recipe == null)
                {
                    Logger.Info($"Recipe with Id={id} not found.");
                }
                else
                {
                    Logger.Info($"Retrieved ingredient: {recipe}");
                }
                return recipe;
            } catch (Exception ex)
            {
                Logger.Error(ex, $"Error fetching recipe Id={id}");
                throw;
            }
        }

        public PagedResult<Recipe> GetPagedBySearch(int kitchenId, int? recipeCategoryId, string query, int page, int pageSize)
        {
            string pagedBySearchSql = @"
                                        SELECT *
                                        FROM Recipes
                                        WHERE 
                                            KitchenId = @KitchenId
                                            AND (@RecipeCategoryId IS NULL OR RecipeCategoryId = @RecipeCategoryId)
                                            AND (@Query IS NULL OR Title LIKE '%' + @Query + '%')
                                        ORDER BY Title
                                        OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
                                        ";
            string totalCountSql = @"
                                    SELECT COUNT(*)
                                    FROM Recipes
                                    WHERE 
                                        KitchenId = @KitchenId
                                        AND (@RecipeCategoryId IS NULL OR RecipeCategoryId = @RecipeCategoryId)
                                        AND (@Query IS NULL OR Title LIKE '%' + @Query + '%');
                                    ";

            try
            {
                int offset = (page - 1) * pageSize;

                var parameters = new
                {
                    KitchenId = kitchenId,
                    RecipeCategoryId = recipeCategoryId,
                    Query = string.IsNullOrWhiteSpace(query) ? null : query,
                    Offset = offset,
                    PageSize = pageSize
                };

                IEnumerable<Recipe> items = _connection.Query<Recipe>(pagedBySearchSql, parameters);
                int totalCount = _connection.ExecuteScalar<int>(totalCountSql, parameters);

                Logger.Info($"Paged search: items={items.Count()}, total={totalCount}, page={page}");
                return new PagedResult<Recipe>
                {
                    Items = items.ToList(),
                    TotalCount = totalCount,
                    PageNumber = page,
                    PageSize = pageSize
                };
            } catch (Exception ex)
            {
                Logger.Error($"Error while paginating recipes");
                throw;
            }
        }

        public int Add(Recipe recipe)
        {
            string sql = @"INSERT INTO Recipes
                            (KitchenId, RecipeCategoryId, Title, Description, Portions)
                            VALUES
                            (@KitchenId, @RecipeCategoryId, @Title, @Description, @Portions)
                            SELECT CAST(SCOPE_IDENTITY() AS INT)";

            try
            {
                int id = _connection.ExecuteScalar<int>(sql, recipe);
                Logger.Info($"Added recipe: {recipe.Title} with Id={id}");
                return id;
            } catch(Exception ex)
            {
                Logger.Error(ex, $"Error adding recipe: {recipe.Title}");
                throw;
            }
        }

        public bool Update(Recipe recipe)
        {
            string sql = @"UPDATE Recipes
                            SET KitchenId = @KitchenId,
                                RecipeCategoryId = @RecipeCategoryId,
                                Title = @Title,
                                Description = @Description,
                                Portions = @Portions
                            WHERE Id = @Id";

            try
            {
                int affectedRows = _connection.Execute(sql, recipe);
                Logger.Info($"Updated recipe Id={recipe.Id}. Rows affected: {affectedRows}");
                return affectedRows == 1;
            } catch (Exception ex)
            {
                Logger.Error(ex, $"Error updating recipe Id={recipe.Id}");
                throw;
            }
        }

        public bool Delete (int id)
        {
            string sql = "DELETE FROM Recipes WHERE Id = @Id";

            try
            {
                int affectedRows = _connection.Execute(sql, new { Id = id });
                Logger.Info($"Deleted recipe Id={id}, rows affected: {affectedRows}");
                return affectedRows == 1;
            } catch (Exception ex)
            {
                Logger.Error(ex, $"Error deleting recipe Id={id}");
                throw;
            }
        }
    }
}
