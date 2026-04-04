using DummyPos.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Payment")]
public class Payment
{
    [Key]
    public int Payment_Id { get; set; }

    [Required]
    public int Order_Id { get; set; }

    [ForeignKey("Order_Id")]
    public Orders Order { get; set; }

    [Required]
    public int Payment_Type_Id { get; set; }

    [ForeignKey("Payment_Type_Id")]
    public PaymentType PaymentType { get; set; }

    [Required]
    public decimal Final_Amount { get; set; }

    [Required]
    public decimal GST_Amount { get; set; }

    public DateTime? Payment_Date { get; set; }
}