using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheKitchen.Data.Entities;
using TheKitchen.Data.Infrastructure;

namespace TheKitchen.Data.Abstractions
{
    public interface IRecipeRepository
    {
        Recipe GetById(int id);
        PagedResult<Recipe> GetPagedBySearch(int kitchenId, int? recipeCategoryId, string query, int page, int pageSize);
        public int Add(Recipe recipe);
        public bool Update(Recipe recipe);
        public bool Delete(int id);
    }
}
