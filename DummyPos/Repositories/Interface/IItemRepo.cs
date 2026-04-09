using DummyPos.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace DummyPos.Repositories.Interface
{
    public interface IItemRepo
    {
        List<Item> GetAllItems();
        Item GetItemById(int id);
        void AddItem(Item item);
        void UpdateItem(Item item);
        void DeleteItem(int id);
        void ActivateItem(int id);
        void DeactivateItem(int id);
        //
        List<ItemSearchViewModel> SearchAdminItems(string searchTerm);
        //

        // This is required to populate the dropdown list for Item Categories in Create/Edit forms
        List<SelectListItem> GetCategoryDropdown();
    }
}