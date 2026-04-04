using DummyPos.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace DummyPos.Repositories.Interface
{
    public interface IItemGstRateRepo
    {
        List<ItemGstRate> GetAllItemGstRates();
        ItemGstRate GetItemGstRateById(int id);
        void AddItemGstRate(ItemGstRate itemGstRate);
        void UpdateItemGstRate(ItemGstRate itemGstRate);
        void DeleteItemGstRate(int id);

        // Dropdown helpers
        List<SelectListItem> GetItemsDropdown();
        List<SelectListItem> GetServiceTypesDropdown();
    }
}