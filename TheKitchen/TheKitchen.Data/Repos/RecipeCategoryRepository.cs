using System;
using TheKitchen.Data.Abstractions;
using TheKitchen.Data.Entities;
using TheKitchen.Data.Infrastructure;
using System.Data;
using NLog;
using Dapper;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Linq;

namespace TheKitchen.Data.Repos
{
    public class RecipeCategoryRepository : IRecipeCategoryRepository
    {
        IDbConnection _connection;

        private readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public RecipeCategoryRepository(IDbConnection connection)
        {
            _connection = connection;
        }
        public RecipeCategory GetById(int id)
        {
            string sql = "SELECT * FROM RecipeCategories WHERE Id = @Id";

            try
            {
                RecipeCategory category = _connection.QueryFirstOrDefault<RecipeCategory>(sql, new { Id = id });
                if (category != null)
                {
                    Logger.Info($"RecipeCategory with Id={id} not found.");
                } else
                {
                    Logger.Info($"Retrieved ingredient: {category}");
                }
                return category;
            } catch(Exception ex)
            {
                Logger.Error(ex, $"Error fetching ingredient Id={id}");
                throw;
            }
        }
        public PagedResult<RecipeCategory> GetPagedBySearch(int kitchenId, int? parentRecipeCategoryId, string query, int page, int pageSize)
        {
            string pagedBySearchSql = @"
                                    SELECT *
                                    FROM RecipeCategories
                                    WHERE KitchenId = @KitchenId
                                        AND (@ParentRecipeCategoryId IS NULL OR ParentRecipeCategoryId = @ParentRecipeCategoryId)
                                        AND (@Query IS NULL OR Name LIKE '%' + @Query + '%')
                                    ORDER BY Name
                                    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
                                    ";
            string totalCountSql = @"
                                    SELECT COUNT(*)
                                    FROM RecipeCategories
                                    WHERE 
                                        KitchenId = @KitchenId
                                        AND (@ParentRecipeCategoryId IS NULL OR ParentRecipeCategoryId = @ParentRecipeCategoryId)
                                        AND (@Query IS NULL OR Name LIKE '%' + @Query + '%');
                                ";

            try
            {
                int offset = (page - 1) * pageSize;
                var parameters = new
                {
                    KitchenId = kitchenId,
                    ParentRecipeCategoryId = parentRecipeCategoryId,
                    Query = string.IsNullOrWhiteSpace(query) ? null : query,
                    Offset = offset,
                    PageSize = pageSize
                };
                IEnumerable<RecipeCategory> items = _connection.Query<RecipeCategory>(pagedBySearchSql, parameters);
                int totalCount = _connection.ExecuteScalar<int>(totalCountSql, parameters);

                Logger.Info($"Paged search categories: items={items.Count()}, total={totalCount}, page={page}");
                return new PagedResult<RecipeCategory> {
                    Items = items.ToList(),
                    TotalCount = totalCount,
                    PageNumber = page,
                    PageSize = pageSize
                };
            } catch (Exception ex)
            {
                Logger.Error(ex, $"Error while paginating ingredient categories");
                throw;
            }
        }
        public int Add(RecipeCategory category)
        {
            string sql = @"INSERT INTO RecipeCategories (KitchenId, ParentRecipeCategoryId, Name)
                            VALUES(@KitchenId, @ParentRecipeCategoryId, @Name)
                            SELECT CAST(SCOPE_IDENTITY() AS INT)";
            try
            {
                int id = _connection.ExecuteScalar<int>(sql, category);
                Logger.Info($"Added category: {category.Name} with Id={id}");
                return id;
            } catch (Exception ex)
            {
                Logger.Error(ex, $"Error adding category: {category.Name}");
                throw;
            }
        }
        public bool Update(RecipeCategory category)
        {
            string sql = @"UPDATE RecipeCategories
                            SET KitchenId = @KitchenId,
                                ParentRecipeCategoryId = @ParentRecipeCategoryId,
                                Name = @Name
                            WHERE Id = @Id";

            try
            {
                int affectedRows = _connection.Execute(sql, category);
                Logger.Info($"Updated category Id={category.Id}. Rows affected: {affectedRows}");
                return affectedRows == 1;
            } catch (Exception ex)
            {
                Logger.Error(ex, $"Error updating category Id={category.Id}");
                throw;
            }
        }
        public bool Delete(int id)
        {
            string sql = "DELETE FROM RecipeCategories WHERE Id = @Id";
            try
            {
                int affectedRows = _connection.Execute(sql, new { Id = id });
                Logger.Info($"Deleted category Id={id}. Rows affected: {affectedRows}");
                return affectedRows == 1;
            } catch (Exception ex)
            {
                Logger.Error(ex, $"Error deleting category Id={id}");
                throw;
            }
        }
    }
}
