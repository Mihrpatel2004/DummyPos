using DummyPos.Models;

namespace DummyPos.Repositories.Interface
{
    public interface IItemCatRepo
    {
        public Item GetItemById(int id);
    }
}
