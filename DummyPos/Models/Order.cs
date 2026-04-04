using System;
using System.ComponentModel.DataAnnotations;

namespace DummyPos.Models
{
    public class Order
    {
        [Key]
        public int Order_Id { get; set; }

        [Display(Name = "Order Number")]
        public string? Order_No { get; set; } // Auto-generated like "ORD-2026..."

        public int? Customer_Id { get; set; }

        [Required]
        public int Staff_Id { get; set; }

        public DateTime Order_Date { get; set; } = DateTime.Now;

        [Display(Name = "Expected Ready Time")]
        public DateTime? Expected_Ready_Time { get; set; }

        // --- Financial Snapshots ---
        public decimal SubTotal { get; set; } = 0;
        public decimal TaxAmount { get; set; } = 0;
        public decimal DiscountAmount { get; set; } = 0;
        public decimal GrandTotal { get; set; } = 0;

        // --- Foreign Keys / Relationships ---
        [Required]
        public int Status_Id { get; set; } // 1 = Pending, 2 = Completed, etc.

        [Required]
        public int Source_Id { get; set; } // Dine-in, Takeaway, Zomato

        [Required]
        public int Branch_Id { get; set; }

        [Required]
        public int Service_Type_Id { get; set; }

        public int? Table_Id { get; set; }
    }
}