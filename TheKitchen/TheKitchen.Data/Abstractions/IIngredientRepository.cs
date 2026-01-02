using System.Collections.Generic;
using TheKitchen.Data.Entities;
using TheKitchen.Data.Infrastructure;

namespace TheKitchen.Data.Abstractions
{
    public interface IIngredientRepository
    {
        // IEnumerable<Ingredient> GetByRecipeId(int id);
        Ingredient GetById(int id);
        PagedResult<Ingredient> GetPagedBySearch(int kitchenId, int? categoryId, string query, int page, int pageSize);
        int Add(Ingredient ingredient);
        bool Update(Ingredient ingredient);
        bool Delete(int id);
    }
}
