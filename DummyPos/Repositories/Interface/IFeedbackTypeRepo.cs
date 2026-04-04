using DummyPos.Models;
using System.Collections.Generic;

namespace DummyPos.Repositories.Interface
{
    public interface IFeedbackTypeRepo
    {
        List<FeedbackType> GetAllFeedbackTypes();
        FeedbackType GetFeedbackTypeById(int id);
        void AddFeedbackType(FeedbackType feedbackType);
        void UpdateFeedbackType(FeedbackType feedbackType);
        void DeleteFeedbackType(int id);
    }
}