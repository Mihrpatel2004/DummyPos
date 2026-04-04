using DummyPos.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace DummyPos.Repositories.Interface
{
    public interface IScreenRepo
    {
        List<ItemDisplayScreen> GetScreensByStationId(int stationId);
        ItemDisplayScreen GetScreenById(int id);
        void AddScreen(ItemDisplayScreen screen);
        void UpdateScreen(ItemDisplayScreen screen);
        void DeleteScreen(int id);
        public void ActivateScreen(int id);
        public void DeactivateScreen(int id);
        List<SelectListItem> GetItemsDropdown();
    }
}