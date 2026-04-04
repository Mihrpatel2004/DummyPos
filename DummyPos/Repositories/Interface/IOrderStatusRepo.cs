using DummyPos.Models;
using System.Collections.Generic;

namespace DummyPos.Repositories.Interface
{
    public interface IOrderStatusRepo
    {
        List<OrderStatus> GetAllOrderStatuses();
        OrderStatus GetOrderStatusById(int id);
        void AddOrderStatus(OrderStatus orderStatus);
        void UpdateOrderStatus(OrderStatus orderStatus);
        void DeleteOrderStatus(int id);
        void ActivateOrderStatus(int id);
        void DeactivateOrderStatus(int id);
    }
}