using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheKitchen.Data.Entities
{
    public class Ingredient
    {
		public int Id { get; set; }
		public int KitchenId { get; set; }
		public int? CategoryId { get; set; }
		public string Title { get; set; }
		public int? BaseUnit { get; set; }
		public decimal StockQuantity { get; set; } = default;
    }
}
