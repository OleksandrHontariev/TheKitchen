using Dapper;
using NLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheKitchen.Data.Abstractions;
using TheKitchen.Data.Entities;

namespace TheKitchen.Data.Repos
{
    public class RecipeDetailRepository : IRecipeDetailRepository
    {
        private readonly Logger Logger = LogManager.GetCurrentClassLogger();
        IDbConnection _connection;
        public RecipeDetailRepository(IDbConnection connection)
        {
            _connection = connection;
        }
        public IEnumerable<RecipeDetail> GetByRecipeId(int kitchenId, int recipeId)
        {
            string sql = @"SELECT
                            Id,
                            KitchenId,
                            Ordering,
                            RecipeId,
                            IngredientId,
                            UnitId,
                            Quantity
                        FROM RecipeDetails
                        WHERE KitchenId = @KitchenId AND RecipeId = @RecipeId
                        ORDER BY Ordering";
            try
            {
                IEnumerable<RecipeDetail> data = _connection.Query<RecipeDetail>(sql,
                    new {
                        KitchenId = kitchenId,
                        RecipeId = recipeId
                    });
                Logger.Info($"Getting list of recipe details of length {data.Count()}");
                return data;

            } catch (Exception ex)
            {
                Logger.Error(ex, "Error while getting list of recipe details");
                throw;
            }
        }

        public RecipeDetail GetById(int id)
        {
            string sql = @"SELECT
                            Id,
                            KitchenId,
                            Ordering,
                            RecipeId,
                            IngredientId,
                            UnitId,
                            Quantity
                        FROM RecipeDetails
                        WHERE Id = @Id";
            RecipeDetail recipeDetail = null;
            try
            {
                recipeDetail = _connection.QueryFirstOrDefault<RecipeDetail>(sql, new { Id = id });
                if (recipeDetail == null)
                {
                    Logger.Info($"Recipe detail with Id={id} not found.");
                } else
                {
                    Logger.Info($"Retrieved recipe detail: {recipeDetail}");
                }
                return recipeDetail;

            } catch (Exception ex)
            {
                Logger.Error(ex, $"Error fetching recipe detail Id={id}");
                throw;
            }
        }
        public int Add(RecipeDetail recipeDetail)
        {
            string sql = @"INSERT INTO RecipeDetails (KitchenId, Ordering, RecipeId, IngredientId, UnitId, Quantity)
                        VALUES (@KitchenId, @Ordering, @RecipeId, @IngredientId, @UnitId, @Quantity)
                        SELECT CAST(SCOPE_IDENTITY() AS INT)";
            try
            {
                int id = _connection.ExecuteScalar<int>(sql, recipeDetail);
                Logger.Info($"Added recipe detail with Id={id}");
                return id;
            } catch (Exception ex)
            {
                Logger.Error(ex, $"Error adding recipe detail");
                throw;
            }
        }

        public bool Update(RecipeDetail recipeDetail)
        {
            string sql = @"UPDATE RecipeDetails
                            SET KitchenId = @KitchenId,
                                RecipeId = @RecipeId,
                                IngredientId = @IngredientId,
                                UnitId = @UnitId,
                                Ordering = @Ordering,
                                Quantity = @Quantity
                            WHERE Id = @Id";

            try
            {
                int affectedRows = _connection.Execute(sql, recipeDetail);
                Logger.Info($"Updated recipe detail Id={recipeDetail.Id}. Rows affected: {affectedRows}");
                return affectedRows == 1;
            } catch (Exception ex)
            {
                Logger.Error(ex, $"Error updating recipe detail Id={recipeDetail.Id}");
                throw;
            }
        }

        public bool Delete(int id)
        {
            string sql = "DELETE FROM RecipeDetails WHERE Id = @Id";
            try
            {
                int affectedRows = _connection.Execute(sql, new { Id = id });
                Logger.Info($"Deleted recipe detail Id={id}, rows affected: {affectedRows}");
                return affectedRows == 1;
            } catch (Exception ex)
            {
                Logger.Error(ex, $"Error deleting recipe detail Id={id}");
                throw;
            }
        }
    }
}
