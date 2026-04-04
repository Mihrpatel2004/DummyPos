using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Toppings")]
public class Toppings
{
    [Key]
    public int Topping_Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Topping_Name { get; set; }

    [Required]
    public decimal Price { get; set; }

    public bool Is_Active { get; set; }

    public DateTime? Created_Date { get; set; }

    [StringLength(100)]
    public string Created_By { get; set; }

    public DateTime? Modified_Date { get; set; }

    [StringLength(100)]
    public string Modified_By { get; set; }
}