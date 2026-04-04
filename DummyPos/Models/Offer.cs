using System;
using System.ComponentModel.DataAnnotations;

namespace DummyPos.Models
{
    public class Offer
    {
        [Key]
        public int Offer_Id { get; set; }

        [Required(ErrorMessage = "Offer Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
        [Display(Name = "Campaign Name")]
        public string? Offer_Name { get; set; }

        [Required(ErrorMessage = "Percentage is required")]
        [Range(0.01, 100.00, ErrorMessage = "Discount must be between 0.01% and 100%")]
        [Display(Name = "Discount Percentage (%)")]
        public decimal Offer_Percent { get; set; }

        [Required(ErrorMessage = "Start Date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Valid From")]
        public DateTime Start_Date { get; set; }

        [Required(ErrorMessage = "End Date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Valid Until")]
        public DateTime End_Date { get; set; }

        [Display(Name = "Active Status")]
        public bool Is_Active { get; set; } = true;

        // Used only to display the dynamic status on the Index page
        public string? Current_Status { get; set; }
    }
}