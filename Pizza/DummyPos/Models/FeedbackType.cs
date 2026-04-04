using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

[Table("Feedback_Type")]
public class FeedbackType
{
    [Key]
    public int Feedback_Type_Id { get; set; }

    [StringLength(100)]
    public string Feedback_Type_Name { get; set; }
}