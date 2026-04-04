using System;

namespace DummyPos.Models
{
    // For the main table list
    public class AdminOrderViewModel
    {
        public int Order_Id { get; set; }
        public string Order_No { get; set; }
        public string Customer_Name { get; set; }
        public string Staff_Name { get; set; }
        public string Branch_Name { get; set; }
        public DateTime Order_Date { get; set; }
        public decimal GrandTotal { get; set; }
        public string Status_Name { get; set; }
    }

    // For the popup details
    public class AdminOrderItemViewModel
    {
        public string Item_Name { get; set; }
        public int Quantity { get; set; }
        public decimal Total_Price { get; set; }
    }
}