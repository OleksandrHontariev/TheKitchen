using TheKitchen.Data.Entities;
using System.Collections.Generic;

namespace TheKitchen.Data.Abstractions
{
    public interface IKitchenRepository
    {
        IEnumerable<Kitchen> GetAll();
        Kitchen GetById(int id);
        int Add(Kitchen kitchen);
        bool Update(Kitchen kitchen);
        bool Delete(int id);
    }
}
