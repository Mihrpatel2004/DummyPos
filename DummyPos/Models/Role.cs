using System;
using System.ComponentModel.DataAnnotations;

namespace DummyPos.Models
{
    public class Role
    {
        [Key]
        public int Role_Id { get; set; }

        [Required(ErrorMessage = "Role description is required")]
        [StringLength(100)]
        [Display(Name = "Role Description")]
        public string? Role_Desc { get; set; }

        [Display(Name = "Active")]
        public bool Is_Active { get; set; } = true;

        [DataType(DataType.DateTime)]
        [Display(Name = "Created Date")]
        public DateTime? Created_Date { get; set; } = DateTime.Now;

        [StringLength(100)]
        [Display(Name = "Created By")]
        public string? Created_By { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Modified Date")]
        public DateTime? Modified_Date { get; set; }

        [StringLength(100)]
        [Display(Name = "Modified By")]
        public string? Modified_By { get; set; }
    }
}