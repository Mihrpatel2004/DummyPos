
    using System;
    using System.ComponentModel.DataAnnotations;

    namespace DummyPos.Models
    {
        public class ServiceType
        {
            [Key]
            public int Service_Type_Id { get; set; }

            [Required(ErrorMessage = "Service Type Description is required")]
            [Display(Name = "Service Description")]
            public string? Service_Type_Desc { get; set; }

            [Display(Name = "Active Status")]
            public bool Is_Active { get; set; } = true;

            [Display(Name = "Created Date")]
            public DateTime? Created_Date { get; set; }
        }
    }
