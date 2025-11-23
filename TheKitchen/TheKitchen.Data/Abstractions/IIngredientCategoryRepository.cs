using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheKitchen.Data.Entities;
using TheKitchen.Data.Infrastructure;

namespace TheKitchen.Data.Abstractions
{
    public interface IIngredientCategoryRepository
    {
        PagedResult<IngredientCategory> GetPagedBySearch(int kitchenId, int? parentCategoryId, string query, int page, int pageSize);
        IngredientCategory GetById(int id);
        int Add(IngredientCategory category);
        bool Update(IngredientCategory category);
        bool Delete(int id);
    }
}
