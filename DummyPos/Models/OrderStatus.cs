using System.ComponentModel.DataAnnotations;

namespace DummyPos.Models
{
    public class OrderStatus
    {
        [Key]
        public int Status_Id { get; set; }

        [Required(ErrorMessage = "Order Status Description is required")]
        [StringLength(100, ErrorMessage = "Description cannot be longer than 100 characters")]
        [Display(Name = "Order Status (e.g., Pending, Preparing, Ready)")]
        public string? Status_Desc { get; set; }

        [Display(Name = "Active Status")]
        public bool Is_Active { get; set; } = true;
    }
}