using System.ComponentModel.DataAnnotations;

namespace DummyPos.Models
{
    public class ItemGstRate
    {
        [Key]
        public int IGR_Id { get; set; }

        [Required(ErrorMessage = "Please select an Item")]
        [Display(Name = "Item")]
        public int Item_Id { get; set; }

        [Required(ErrorMessage = "Please select a Service Type")]
        [Display(Name = "Service Type")]
        public int Service_Type_Id { get; set; }

        [Required(ErrorMessage = "GST Rate is required")]
        [Range(0, 100, ErrorMessage = "Rate must be between 0% and 100%")]
        [Display(Name = "GST Rate (%)")]
        public decimal GST_Rate { get; set; }

        // Used for displaying text in the UI instead of just IDs
        public string? Item_Name { get; set; }
        public string? Service_Type_Desc { get; set; }
    }
}