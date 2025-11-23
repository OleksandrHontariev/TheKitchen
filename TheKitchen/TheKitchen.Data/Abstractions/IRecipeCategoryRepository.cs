using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheKitchen.Data.Entities;
using TheKitchen.Data.Infrastructure;

namespace TheKitchen.Data.Abstractions
{
    public interface IRecipeCategoryRepository
    {
        public PagedResult<RecipeCategory> GetPagedBySearch(int kitchenId, int? categoryId, string query, int page, int pageSize);
        public RecipeCategory GetById(int id);
        public int Add(RecipeCategory recipeCategory);
        public bool Update(RecipeCategory recipeCategory);
        public bool Delete(int id);
    }
}
