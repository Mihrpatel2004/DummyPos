/*using System;
using System.ComponentModel.DataAnnotations;

namespace DummyPos.Models
{
    public class Staffs
    {
       
            [Key]
            public int Staff_Id { get; set; }

            [Required(ErrorMessage = "Staff name is required")]
            [StringLength(100)]
            [Display(Name = "Staff Name")]
            public string? Staff_Name { get; set; }

            [Required(ErrorMessage = "Role is required")]
            [Display(Name = "Role")]
            public int Role_Id { get; set; }
        public string? Role_Name { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
            [Phone]
            [StringLength(15)]
            public string? Phone { get; set; }

            [EmailAddress]
            public string? Email { get; set; }

            [DataType(DataType.Date)]
            [Display(Name = "Hire Date")]
            public DateTime? Hire_Date { get; set; }

            [Range(0, 1000000)]
            public decimal? Salary { get; set; }

            [Required(ErrorMessage = "Branch is required")]
            [Display(Name = "Branch")]
            public int BranchId { get; set; }

        public string? Branch_Name { get; set; }

        public bool Is_Active { get; set; } = true;

            public byte[]? Staff_Password { get; set; }

            [Required(ErrorMessage = "Username is required")]
            [StringLength(50)]
            public string? Username { get; set; }

            public DateTime? Last_Login { get; set; }


    }
  }
*/

using System;
using System.ComponentModel.DataAnnotations;

namespace DummyPos.Models
{
    public class Staffs
    {
        [Key]
        public int Staff_Id { get; set; }

        [Required(ErrorMessage = "Staff name is required")]
        [StringLength(100)]
        [Display(Name = "Staff Name")]
        public string? Staff_Name { get; set; }

        [Required(ErrorMessage = "Role is required")]
        [Display(Name = "Role")]
        public int Role_Id { get; set; }

        // Used to display the text name from the JOIN
        public string? Role_Name { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Phone]
        [StringLength(15)]
        public string? Phone { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Hire Date")]
        public DateTime? Hire_Date { get; set; }

        [Range(0, 1000000)]
        public decimal? Salary { get; set; }

        [Required(ErrorMessage = "Branch is required")]
        [Display(Name = "Branch")]
        public int BranchId { get; set; }

        // Used to display the text name from the JOIN
        public string? Branch_Name { get; set; }

        public bool Is_Active { get; set; } = true;

        [Required(ErrorMessage = "Username is required")]
        [StringLength(50)]
        public string? Username { get; set; }

        // CHANGED: This replaces the old byte[] Staff_Password
        public string? PasswordHash { get; set; }

        [Display(Name = "Last Login")]
        public DateTime? Last_Login { get; set; }
    }
}