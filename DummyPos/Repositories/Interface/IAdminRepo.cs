using DummyPos.Models;

namespace DummyPos.Repositories.Interface
{
    public interface IAdminRepo
    {
        public List<Branch> GetAllBranches();
        public List<StaffDetailViewModel> GetStaffByBranchId(int branchId);
        public StaffDetailViewModel GetStaffDetailsById(int staffId);
        DummyPos.Models.DashboardViewModel GetDashboardMetrics(int? branchId);

        //new 
        List<AdminOrderViewModel> GetFilteredOrders(int branchId, DateTime? startDate, DateTime? endDate);
        List<AdminOrderItemViewModel> GetOrderItemsAdmin(int orderId);
    }
}
