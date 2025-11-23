using System;
using System.Collections.Generic;
using System.Linq;
using TheKitchen.Data.Abstractions;
using TheKitchen.Data.Entities;
using TheKitchen.Data.Infrastructure;
using NLog;
using Dapper;
using System.Data;

namespace TheKitchen.Data.Repos
{
    public class IngredientCategoryRepository : IIngredientCategoryRepository
    {
        IDbConnection _connection;
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public IngredientCategoryRepository(IDbConnection connection)
        {
            _connection = connection;
        }
        public PagedResult<IngredientCategory> GetPagedBySearch(int kitchenId, int? parentCategoryId, string query, int page, int pageSize)
        {
            string pagedBySearchSql = @"
                                    SELECT *
                                    FROM IngredientCategories
                                    WHERE KitchenId = @KitchenId
                                        AND (
                                            (@ParentCategoryId IS NULL AND ParentCategoryId IS NULL) OR
                                            (@ParentCategoryId IS NOT NULL AND ParentCategoryId = @ParentCategoryId)
                                        )
                                        AND (@Query IS NULL OR Name LIKE '%' + @Query + '%')
                                    ORDER BY Name
                                    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
                                    ";
            string totalCountSql = @"
                                    SELECT COUNT(*)
                                    FROM IngredientCategories
                                    WHERE 
                                        KitchenId = @KitchenId
                                        AND (
                                                (@ParentCategoryId IS NULL AND ParentCategoryId IS NULL)
                                             OR (@ParentCategoryId IS NOT NULL AND ParentCategoryId = @ParentCategoryId)
                                            )
                                        AND (@Query IS NULL OR Name LIKE '%' + @Query + '%');
                                ";
            try
            {
                int offset = (page - 1) * pageSize;
                var parameters = new
                {
                    KitchenId = kitchenId,
                    ParentCategoryId = parentCategoryId,
                    Query = string.IsNullOrWhiteSpace(query) ? null : query,
                    Offset = offset,
                    PageSize = pageSize
                };

                IEnumerable<IngredientCategory> items = _connection.Query<IngredientCategory>(pagedBySearchSql, parameters);
                int totalCount = _connection.ExecuteScalar<int>(totalCountSql, parameters);

                Logger.Info($"Paged search categories: items={items.Count()}, total={totalCount}, page={page}");
                return new PagedResult<IngredientCategory> {
                    Items = items,
                    TotalCount = totalCount,
                    PageNumber = page,
                    PageSize = pageSize
                };
            } catch (Exception ex)
            {
                Logger.Error(ex, "Error while paginating ingredient categories");
                throw;
            }
        }
        public IngredientCategory GetById(int id)
        {
            string sql = "SELECT * FROM IngredientCategories WHERE Id = @Id";
            try
            {
                IngredientCategory category = _connection.QueryFirstOrDefault<IngredientCategory>(sql, new { Id = id });
                if (category != null)
                {
                    Logger.Info($"Fetched category: {category.Name} (Id={category.Id})");
                } else
                {
                    Logger.Info($"Category with Id={id} not found");
                }
                return category;
            } catch (Exception ex)
            {
                Logger.Error(ex, $"Error fetching category by Id={id}");
                throw;
            }
        }
        public int Add(IngredientCategory category)
        {
            string sql = @"INSERT INTO IngredientCategories (KitchenId, ParentCategoryId, Name)
                        VALUES (@KitchenId, @ParentCategoryId, @Name)
                        SELECT CAST(SCOPE_IDENTITY() AS INT)";

            try
            {
                int id = _connection.ExecuteScalar<int>(sql, category);
                Logger.Info($"Added category: {category.Name} with Id={id}");
                return id;
            } catch(Exception ex)
            {
                Logger.Error(ex, $"Error adding category: {category.Name}");
                throw;
            }
        }
        public bool Update(IngredientCategory category)
        {
            string sql = @"UPDATE IngredientCategories
                            SET KitchenId = @KitchenId,
                                ParentCategoryId = @ParentCategoryId,
                                Name = @Name
                            WHERE Id = @Id";

            try
            {
                int affectedRows = _connection.Execute(sql, category);
                Logger.Info($"Updated category Id={category.Id}. Rows affected: {affectedRows}");
                return affectedRows == 1;
            } catch(Exception ex)
            {
                Logger.Error($"Error updating category Id={category.Id}");
                throw;
            }
        }
        public bool Delete(int id)
        {
            string sql = "DELETE FROM IngredientCategories WHERE Id = @Id";
            try
            {
                int affectedRows = _connection.Execute(sql, new { Id = id });
                Logger.Info($"Deleted category Id={id}. Rows affected: {affectedRows}");
                return affectedRows == 1;
            } catch(Exception ex)
            {
                Logger.Error(ex, $"Error deleting category Id={id}");
                throw;
            }
        }
    }
}
