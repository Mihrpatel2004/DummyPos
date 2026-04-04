using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DummyPos.Models
{
    public class CheckoutViewModel
    {
        public string Customer_Name { get; set; }
        public string Customer_Phone { get; set; }
        public int Source_Id { get; set; }
        public int Service_Type_Id { get; set; }
        public int Payment_Type_Id { get; set; }
      
        public int? Table_Id { get; set; }
        public string Order_Status { get; set; }
        public decimal SubTotal { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal GrandTotal { get; set; }
        public List<CartItemViewModel> Items { get; set; }
    }

    public class CartItemViewModel
    {
        public int Item_Id { get; set; }
        public string Item_Name { get; set; }
        public int Quantity { get; set; }

        // FIX: Renamed these to perfectly match the JavaScript and the Repository!
        public decimal Unit_Price { get; set; }
        public decimal Total_Price { get; set; }

        public decimal TaxAmount { get; set; }
    }

    // --- ADD THIS NEW CLASS HERE ---
    public class ItemViewModel
    {
        public int Item_Id { get; set; }
        public string Item_Name { get; set; }
        public decimal Amount { get; set; }
        public int Category_Id { get; set; }
        public decimal Gst_Rate { get; set; }
        public byte[] Item_Image { get; set; }

        // This converts the byte[] into an image so your POS screen can display it!
        public string ImageDataUrl
        {
            get
            {
                if (Item_Image != null && Item_Image.Length > 0)
                {
                    string imageBase64 = Convert.ToBase64String(Item_Image);
                    return $"data:image/jpeg;base64,{imageBase64}";
                }
                // Fallback if no image is uploaded
                return "https://via.placeholder.com/150?text=No+Image";
            }
        } 
    }

    public class OpenOrderViewModel
    {
        public int Order_Id { get; set; }
        public string Order_No { get; set; }
        public string Table_Number { get; set; }
        public decimal GrandTotal { get; set; }
        public DateTime Order_Date { get; set; }
    }
    public class KitchenOrderViewModel
    {
        public string Order_No { get; set; }
        public string Table_Number { get; set; }
        public DateTime Order_Date { get; set; }
        public List<KitchenOrderItemViewModel> Items { get; set; } = new List<KitchenOrderItemViewModel>();
    }

    public class KitchenOrderItemViewModel
    {
        public string Item_Name { get; set; }
        public int Quantity { get; set; }
    }

    public class ReceiptViewModel
    {
        public int Order_Id { get; set; }
        public string Order_No { get; set; }
        public DateTime Order_Date { get; set; }
        public string Branch_Name { get; set; }
        public string Staff_Name { get; set; }
        public string Customer_Name { get; set; }
        public string Customer_Phone { get; set; }
        public string Payment_Type { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal GrandTotal { get; set; }
        public List<ReceiptItemViewModel> Items { get; set; } = new List<ReceiptItemViewModel>();
    }

    public class ReceiptItemViewModel
    {
        public string Item_Name { get; set; }
        public int Quantity { get; set; }
        public decimal Unit_Price { get; set; }
        public decimal Total_Price { get; set; }
    }
    public class ManualFeedbackViewModel
    {
        [Required(ErrorMessage = "Phone Number is required to find the order")]
        [Display(Name = "Customer Phone")]
        [StringLength(10, MinimumLength = 10, ErrorMessage = "Phone must be 10 digits")]
        public string Customer_Phone { get; set; }

        public string? Customer_Name { get; set; }

        [Required(ErrorMessage = "Could not find a recent order for this phone number.")]
        public int Order_Id { get; set; }

        [Required]
        [Display(Name = "Feedback Category")]
        public int Feedback_Type_Id { get; set; }

        public int Rating { get; set; } = 5;

        [StringLength(300)]
        public string? Comments { get; set; }
    }

    public class StaffDetailViewModel
    {
        public int Staff_Id { get; set; }
        public string Staff_Name { get; set; }
        public string Role_Desc { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateTime? Hire_Date { get; set; }
        public decimal? Salary { get; set; }
        public string Branch_Name { get; set; }
        public string Username { get; set; }
        public DateTime? Last_Login { get; set; }
        public bool Is_Active { get; set; }
    }
}