using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheKitchen.Data.Entities
{
    public class RecipeDetail
    {
		public int Id { get; set; }
		public int KitchenId { get; set; }
		public int RecipeId { get; set; }
		public int IngredientId { get; set; }
		public int UnitId { get; set; }
        public int Ordering { get; set; }
        public decimal Quantity { get; set; }
		public override string ToString() =>
			$"[Id={Id}, KitchenId={KitchenId}, Ordering={Ordering}, RecipeId={RecipeId}, IngredientId={IngredientId}, Quantity={Quantity}]";
    }
}
