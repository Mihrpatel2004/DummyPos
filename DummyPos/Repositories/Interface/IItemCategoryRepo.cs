using DummyPos.Models;
using System.Collections.Generic;

namespace DummyPos.Repositories.Interface
{
    public interface IItemCategoryRepo
    {
        List<ItemCategory> GetAllItemCategories();
        ItemCategory GetItemCategoryById(int id);
        void AddItemCategory(ItemCategory category);
        void UpdateItemCategory(ItemCategory category);
        void DeleteItemCategory(int id);
        void ActivateItemCategory(int id);
        void DeactivateItemCategory(int id);
    }
}