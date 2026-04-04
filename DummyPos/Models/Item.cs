using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DummyPos.Models
{
    public class Item
    {
        [Key]
        public int Item_Id { get; set; }

        [Required(ErrorMessage = "Item Name is required")]
        [StringLength(100, ErrorMessage = "Item Name cannot exceed 100 characters")]
        [Display(Name = "Item Name")]
        public string Item_Name { get; set; }

        [Required(ErrorMessage = "Amount is required")]
        [Range(0.01, 100000.00, ErrorMessage = "Amount must be greater than zero")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Please select a category")]
        [Display(Name = "Category")]
        public int Item_Category_Id { get; set; }

        [Display(Name = "Prepare Time (Mins)")]
        public int? Prepare_Time_Minutes { get; set; }

        [Display(Name = "Active Status")]
        public bool Is_Active { get; set; } = true;

        // Binary data from database (VARBINARY(MAX))
        public byte[]? Item_Image { get; set; }

        // Category Name (Used for displaying in lists via JOIN, not saved directly to Items table)
        public string? Item_Category_Desc { get; set; }

        // Used ONLY for the file upload form (not saved directly to DB)
        [NotMapped]
        [Display(Name = "Upload Image")]
        public IFormFile? ImageUpload { get; set; }

        // Helper property to convert byte[] to a displayable image URL in HTML views
        public string ImageDataUrl
        {
            get
            {
                if (Item_Image != null && Item_Image.Length > 0)
                {
                    string imageBase64Data = Convert.ToBase64String(Item_Image);
                    return $"data:image/jpeg;base64,{imageBase64Data}";
                }
                // Fallback image if none exists (make sure you have a placeholder image in wwwroot/images/)
                return "/images/no-image.png";
            }
        }
        public decimal Gst_Rate { get; set; }
    }
}