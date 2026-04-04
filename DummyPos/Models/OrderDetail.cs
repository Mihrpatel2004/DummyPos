using System.ComponentModel.DataAnnotations;

namespace DummyPos.Models
{
    public class OrderDetail
    {
        [Key]
        public int Order_Detail_Id { get; set; }

        [Required]
        public int Order_Id { get; set; }

        [Required]
        public int Item_Id { get; set; }

        // We add this just so we can show the name on the screen easily
        public string? Item_Name { get; set; }

        [Required]
        public int Quantity { get; set; }

        // --- Financial Snapshots per item ---
        public decimal Unit_Price { get; set; }
        public decimal Total_Price { get; set; } // Unit_Price * Quantity

        public decimal? Discount { get; set; } = 0;

        public int? Offer_Product_Id { get; set; }
        public int? Topping_Id { get; set; }
    }
}