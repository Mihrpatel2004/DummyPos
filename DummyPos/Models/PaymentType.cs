using System.ComponentModel.DataAnnotations;

namespace DummyPos.Models
{
    public class PaymentType
    {
        [Key]
        public int PT_Id { get; set; }

        [Required(ErrorMessage = "Payment Description is required")]
        [StringLength(100, ErrorMessage = "Description cannot be longer than 100 characters")]
        [Display(Name = "Payment Method")]
        public string? PT_Desc { get; set; }

        [Display(Name = "Active Status")]
        public bool Is_Active { get; set; } = true;
    }
}