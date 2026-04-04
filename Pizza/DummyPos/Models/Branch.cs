using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Branch")]
public class Branch
{
    [Key]
    public int Branch_Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Branch_Name { get; set; }

    public bool Is_Active { get; set; }

    public DateTime? Created_Date { get; set; }

    [StringLength(100)]
    public string Created_By { get; set; }

    public DateTime? Modified_Date { get; set; }

    [StringLength(100)]
    public string Modified_By { get; set; }
}