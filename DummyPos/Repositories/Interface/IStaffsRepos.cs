/*using DummyPos.Models;
using static DummyPos.Models.ItemViewModel;

namespace DummyPos.Repositories.Interface
{
    public interface IStaffsRepos
    {
        string PlaceOrder(CheckoutViewModel model, int staffId, int branchId);

        string  GetBranchNameByStaffId(int staffId);

        string GetCustomerNameByPhone(string phone);
        bool UpdateOrder(int orderId, CheckoutViewModel model);
        CheckoutViewModel GetOrderDetailsById(int orderId);
        ReceiptViewModel GetReceipt(int orderId);
        List<KitchenOrderViewModel> GetKitchenOrders(int branchId);
        List<OpenOrderViewModel> GetOpenOrders(int branchId);


    }
}
*//*
using DummyPos.Models;
using static DummyPos.Models.ItemViewModel;
using System.Collections.Generic;

namespace DummyPos.Repositories.Interface
{
    public interface IStaffsRepos
    {
        List<RestaurantTable> GetTablesForPos(int branchId);
        string PlaceOrder(CheckoutViewModel model, int staffId, int branchId);
        string GetBranchNameByStaffId(int staffId);
        string GetCustomerNameByPhone(string phone);
        bool UpdateOrder(int orderId, CheckoutViewModel model);
        CheckoutViewModel GetOrderDetailsById(int orderId);
        ReceiptViewModel GetReceipt(int orderId);
        List<KitchenOrderViewModel> GetKitchenOrders(int branchId);
        List<OpenOrderViewModel> GetOpenOrders(int branchId);

        // Add this line anywhere inside the interface
        List<DummyPos.Models.PaymentType> GetActivePaymentTypes();
        List<DummyPos.Models.OrderSource> GetActiveOrderSources();

        int GetBranchIdByStaffId(int staffId);
        public List<FeedbackType> GetFeedbackTypesForPos();
        public bool SaveOrderFeedback(int orderId, int feedbackTypeId, int rating, string comments);
        // 🚨 NEW: Fetch the discount percentage
        decimal GetTodayActiveDiscount();
    }
}
*/
using DummyPos.Models;
using System.Collections.Generic;

namespace DummyPos.Repositories.Interface
{
    public interface IStaffsRepos
    {
        List<RestaurantTable> GetTablesForPos(int branchId);

        // 🚨 FIXED: Now returns an int (the actual order ID)
        int PlaceOrder(CheckoutViewModel model, int staffId, int branchId);

        string GetBranchNameByStaffId(int staffId);
        string GetCustomerNameByPhone(string phone);
        bool UpdateOrder(int orderId, CheckoutViewModel model);
        CheckoutViewModel GetOrderDetailsById(int orderId);
        ReceiptViewModel GetReceipt(int orderId);
        List<KitchenOrderViewModel> GetKitchenOrders(int branchId);
        List<OpenOrderViewModel> GetOpenOrders(int branchId);

        List<PaymentType> GetActivePaymentTypes();
        List<OrderSource> GetActiveOrderSources();

        int GetBranchIdByStaffId(int staffId);
        List<FeedbackType> GetFeedbackTypesForPos();
        bool SaveOrderFeedback(int orderId, int feedbackTypeId, int rating, string comments);
        decimal GetTodayActiveDiscount();


    }
}