using DummyPos.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Feedbacks")]
public class FeedBack
{
    [Key]
    public int Feedback_Id { get; set; }

    public int? Feedback_Type_Id { get; set; }

    [ForeignKey("Feedback_Type_Id")]
    public FeedbackType FeedbackType { get; set; }

    public int? Customer_Id { get; set; }

    [ForeignKey("Customer_Id")]
    public Customers Customer { get; set; }

    public int? Order_Id { get; set; }

    [ForeignKey("Order_Id")]
    public Orders Order { get; set; }

    public int? Item_Id { get; set; }

    [ForeignKey("Item_Id")]
    public Items Item { get; set; }

    [StringLength(300)]
    public string Feedback_Desc { get; set; }

    public int? Rating { get; set; }

    public DateTime? Created_Date { get; set; }
}