using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheKitchen.Data.Entities
{
    public class Recipe
    {
        public int Id { get; set; }
        public int KitchenId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Portions { get; set; }
    }
}
