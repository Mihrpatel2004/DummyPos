using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Customers")]
public class Customers
{
    [Key]
    public int Customer_Id { get; set; }

    [Required]
    [StringLength(100)]
    public string First_Name { get; set; }

    [StringLength(100)]
    public string Last_Name { get; set; }

    [Required]
    [StringLength(15)]
    public string Phone { get; set; }

    [EmailAddress]
    public string Email { get; set; }
}