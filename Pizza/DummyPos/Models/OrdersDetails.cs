using DummyPos.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Order_Details")]
public class OrderDetail
{
    [Key]
    public int Order_Detail_Id { get; set; }

    [Required]
    public int Order_Id { get; set; }

    [ForeignKey("Order_Id")]
    public Orders Order { get; set; }

    [Required]
    public int Item_Id { get; set; }

    [ForeignKey("Item_Id")]
    public Items Item { get; set; }

    [Required]
    public int Quantity { get; set; }

    public decimal? Discount { get; set; }

    public int? Offer_Product_Id { get; set; }

    [ForeignKey("Offer_Product_Id")]
    public OfferProducts OfferProduct { get; set; }

    public int? Topping_Id { get; set; }

    [ForeignKey("Topping_Id")]
    public Toppings Topping { get; set; }
}