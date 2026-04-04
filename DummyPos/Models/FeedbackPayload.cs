namespace DummyPos.Models
{
    public class FeedbackPayload
    {
        public int OrderId { get; set; }
        public int FeedbackTypeId { get; set; }
        public int Rating { get; set; }
        public string Comments { get; set; }
    }
}