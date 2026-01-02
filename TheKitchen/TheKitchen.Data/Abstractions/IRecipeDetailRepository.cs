using System.Collections.Generic;
using TheKitchen.Data.Entities;

namespace TheKitchen.Data.Abstractions
{
    public interface IRecipeDetailRepository
    {
        IEnumerable<RecipeDetail> GetByRecipeId(int kitchenId, int recipeId);
        RecipeDetail GetById(int id);
        int Add(RecipeDetail category);
        bool Update(RecipeDetail category);
        bool Delete(int id);
    }
}
