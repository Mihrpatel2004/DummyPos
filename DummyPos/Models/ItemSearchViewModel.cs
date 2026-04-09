using System.Collections.Generic;

namespace DummyPos.Models
{
    // 1. YOUR EXISTING MODEL (Safely updated)
    public class ItemSearchViewModel
    {
        // Your existing properties (DO NOT CHANGE THESE)
        public int Item_Id { get; set; }
        public string Item_Name { get; set; }
        public decimal Amount { get; set; }
        public string ImageDataUrl { get; set; }

        // NEW properties added for the Admin panel
        // (The Staff panel will just ignore these, so it won't break!)
        public string Category { get; set; }
        public int? Prepare_Time_Minutes { get; set; }
        public bool Is_Active { get; set; }
    }

    // 2. NEW WRAPPER MODEL (Just for the Admin Page)
    public class AdminManageItemsViewModel
    {
        public string SearchTerm { get; set; }
        public List<ItemSearchViewModel> ItemList { get; set; } = new List<ItemSearchViewModel>();
    }
}