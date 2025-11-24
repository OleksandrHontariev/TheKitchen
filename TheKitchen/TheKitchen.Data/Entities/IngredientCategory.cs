using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheKitchen.Data.Entities
{
    public class IngredientCategory
    {
        public int Id { get; set; }
        public int KitchenId { get; set; }
        public int? ParentIngredientCategoryId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public override string ToString() => $"[Id={Id}, KitchenId=\"{KitchenId}\", ParentCategoryId=\"{ParentIngredientCategoryId}\", Name={Name}, CreatedAt={CreatedAt:yyyy-MM-dd HH:mm:ss}]";
    }
}
