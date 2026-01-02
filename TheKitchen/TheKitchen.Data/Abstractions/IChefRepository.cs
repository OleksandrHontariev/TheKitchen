using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheKitchen.Data.Entities;

namespace TheKitchen.Data.Abstractions
{
    public interface IChefRepository
    {
        IEnumerable<Chef> SearchChefs(string query);
        public Chef GetById(int id);
        int Add(Chef chef);
        bool Update(Chef chef);
        bool Delete(int id);
    }
}
