using DummyPos.Models;
using System.Collections.Generic;

namespace DummyPos.Repositories.Interface
{
    public interface IOrderSourceRepo
    {
        List<OrderSource> GetAllOrderSources();
        OrderSource GetOrderSourceById(int id);
        void AddOrderSource(OrderSource orderSource);
        void UpdateOrderSource(OrderSource orderSource);
        void DeleteOrderSource(int id);
        void ActivateOrderSource(int id);
        void DeactivateOrderSource(int id);
    }
}