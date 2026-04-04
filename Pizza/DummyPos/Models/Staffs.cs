using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Staffs")]
public class Staffs
{
    [Key]
    public int Staff_Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Staff_Name { get; set; }

    [Required]
    public int Role_Id { get; set; }

    [ForeignKey("Role_Id")]
    public Role Role { get; set; }

    [StringLength(15)]
    public string Phone { get; set; }

    [EmailAddress]
    public string Email { get; set; }

    public DateTime? Hire_Date { get; set; }

    public decimal? Salary { get; set; }

    public int? BranchId { get; set; }

    [ForeignKey("BranchId")]
    public Branch Branch { get; set; }

    public bool Is_Active { get; set; }

    public DateTime? Created_Date { get; set; }

    [StringLength(100)]
    public string Created_By { get; set; }

    public DateTime? Modified_Date { get; set; }

    [StringLength(100)]
    public string Modified_By { get; set; }
    public string Staff_Password { get; set; }

}