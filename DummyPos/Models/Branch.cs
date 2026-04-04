using System;
using System.ComponentModel.DataAnnotations;

namespace DummyPos.Models
{
    public class Branch
    {
        [Key]
        public int Branch_Id { get; set; }

        [Required(ErrorMessage = "Branch name is required")]
        [StringLength(100)]
        [Display(Name = "Branch Name")]
        public string? Branch_Name { get; set; }

        [Display(Name = "Active")]
        public bool Is_Active { get; set; } = true;

        [DataType(DataType.DateTime)]
        [Display(Name = "Created Date")]
        public DateTime? Created_Date { get; set; } = DateTime.Now;
    }
}