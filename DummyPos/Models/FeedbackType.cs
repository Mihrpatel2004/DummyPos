using System.ComponentModel.DataAnnotations;

namespace DummyPos.Models
{
    public class FeedbackType
    {
        [Key]
        public int Feedback_Type_Id { get; set; }

        [Required(ErrorMessage = "Feedback Type Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
        [Display(Name = "Feedback Type")]
        public string? Feedback_Type_Name { get; set; }
    }
}