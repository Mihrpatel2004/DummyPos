using System.ComponentModel.DataAnnotations;

namespace DummyPos.Models
{
    public class OrderSource
    {
        [Key]
        public int Source_Id { get; set; }

        [Required(ErrorMessage = "Order Source Description is required")]
        [StringLength(100, ErrorMessage = "Description cannot be longer than 100 characters")]
        [Display(Name = "Order Source (e.g., Zomato, Swiggy, Dine-In)")]
        public string? Source_Desc { get; set; }

        [Display(Name = "Active Status")]
        public bool Is_Active { get; set; } = true;
    }
}