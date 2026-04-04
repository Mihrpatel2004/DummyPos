using DummyPos.Models;
using System.Collections.Generic;

namespace DummyPos.Repositories.Interface
{
    public interface ITableRepo
    {
        List<RestaurantTable> GetTablesByBranch(int branchId);
        RestaurantTable GetTableById(int id);
        void AddTable(RestaurantTable table);
        void UpdateTable(RestaurantTable table);
        void DeleteTable(int id);
    }
}