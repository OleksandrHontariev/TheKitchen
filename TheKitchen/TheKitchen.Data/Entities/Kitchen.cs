using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheKitchen.Data.Entities
{
    public class Kitchen
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? TablesCount { get; set; }
        public DateTime CreatedAt { get; set; }
        public override string ToString() =>
            $"[Id={Id}, Name=\"{Name}\", Description=\"{Description}\", TablesCount={TablesCount}, CreatedAt={CreatedAt:yyyy-MM-dd HH:mm:ss}]";
    }
}
