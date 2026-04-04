using System.ComponentModel.DataAnnotations;

namespace DummyPos.Models
{
    public class RestaurantTable
    {
        [Key]
        public int Table_Id { get; set; }

        public int Branch_Id { get; set; }

        [Required(ErrorMessage = "Table Number/Name is required")]
        [Display(Name = "Table Number (e.g., T-1, Patio-2)")]
        public string? Table_Number { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [Display(Name = "Description/Location")]
        public string? Table_Desc { get; set; }

        [Required(ErrorMessage = "Capacity is required")]
        [Range(1, 50, ErrorMessage = "Size must be between 1 and 50")]
        [Display(Name = "Seating Capacity")]
        public int Size { get; set; }

        [Display(Name = "Current Status")]
        public string Status { get; set; } = "Available";
    }
}