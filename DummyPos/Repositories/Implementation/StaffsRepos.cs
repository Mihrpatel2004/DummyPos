using DummyPos.Data;
using DummyPos.Models;
using DummyPos.Repositories.Interface;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace DummyPos.Repositories.Implementation
{
    public class StaffsRepos : IStaffsRepos
    {
        private readonly DbHelper _dbHelper;

        public StaffsRepos(DbHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public string GetBranchNameByStaffId(int staffId)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                string query = @"
                    SELECT b.Branch_Name 
                    FROM Branch b
                    INNER JOIN Staffs s ON b.Branch_Id = s.BranchId
                    WHERE s.Staff_Id = @StaffId";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@StaffId", staffId);

                con.Open();
                object result = cmd.ExecuteScalar();

                return result != null ? result.ToString() : "Unknown Branch";
            }
        }

        public int GetBranchIdByStaffId(int staffId)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                string query = "SELECT BranchId FROM Staffs WHERE Staff_Id = @StaffId";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@StaffId", staffId);

                    con.Open();
                    object result = cmd.ExecuteScalar();

                    return result != null && result != DBNull.Value ? Convert.ToInt32(result) : 1;
                }
            }
        }

        public List<ItemViewModel> GetActiveMenuItems()
        {
            List<ItemViewModel> items = new List<ItemViewModel>();
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                string query = @"
                    SELECT i.Item_Id, i.Item_Name, i.Amount, i.Item_Category_Id, i.Item_Image, 
                           ISNULL((SELECT TOP 1 GST_Rate FROM Item_GST_Rate WHERE Item_Id = i.Item_Id), 0) AS GST_Rate
                    FROM Items i 
                    WHERE i.Is_Active = 1";

                SqlCommand cmd = new SqlCommand(query, con);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    items.Add(new ItemViewModel
                    {
                        Item_Id = Convert.ToInt32(reader["Item_Id"]),
                        Item_Name = reader["Item_Name"].ToString(),
                        Amount = Convert.ToDecimal(reader["Amount"]),
                        Category_Id = Convert.ToInt32(reader["Item_Category_Id"]),
                        Gst_Rate = reader["GST_Rate"] != DBNull.Value ? Convert.ToDecimal(reader["GST_Rate"]) : 0,
                        Item_Image = reader["Item_Image"] != DBNull.Value ? (byte[])reader["Item_Image"] : null
                    });
                }
            }
            return items;
        }

        // 🚨 Returns INT so Javascript can redirect to the Receipt perfectly
        public int PlaceOrder(CheckoutViewModel model, int staffId, int branchId)
        {
            string generatedOrderNo = "ORD-" + DateTime.Now.ToString("yyyyMMddHHmmss");
            int newOrderId = 0;

            using (SqlConnection con = _dbHelper.GetConnection())
            {
                con.Open();
                using (SqlTransaction transaction = con.BeginTransaction())
                {
                    try
                    {
                        int? finalCustomerId = null;

                        if (!string.IsNullOrEmpty(model.Customer_Phone))
                        {
                            string checkQuery = "SELECT Customer_Id FROM Customers WHERE Phone = @Phone";
                            using (SqlCommand checkCmd = new SqlCommand(checkQuery, con, transaction))
                            {
                                checkCmd.Parameters.AddWithValue("@Phone", model.Customer_Phone);
                                object existingId = checkCmd.ExecuteScalar();

                                if (existingId != null)
                                {
                                    finalCustomerId = Convert.ToInt32(existingId);
                                }
                                else
                                {
                                    string insertCustQuery = "INSERT INTO Customers (Customer_Name, Phone) OUTPUT INSERTED.Customer_Id VALUES (@Name, @Phone)";
                                    using (SqlCommand insertCmd = new SqlCommand(insertCustQuery, con, transaction))
                                    {
                                        insertCmd.Parameters.AddWithValue("@Name", string.IsNullOrEmpty(model.Customer_Name) ? "Walk-In" : model.Customer_Name);
                                        insertCmd.Parameters.AddWithValue("@Phone", model.Customer_Phone);
                                        finalCustomerId = Convert.ToInt32(insertCmd.ExecuteScalar());
                                    }
                                }
                            }
                        }

                        if (model.Order_Status == "Open" && model.Table_Id.HasValue && model.Table_Id.Value > 0)
                        {
                            string checkTableQuery = "SELECT COUNT(1) FROM Orders WHERE Table_Id = @TableId AND Order_Status = 'Open' AND Branch_Id = @BranchId";
                            using (SqlCommand checkTableCmd = new SqlCommand(checkTableQuery, con, transaction))
                            {
                                checkTableCmd.Parameters.AddWithValue("@TableId", model.Table_Id.Value);
                                checkTableCmd.Parameters.AddWithValue("@BranchId", branchId);
                                if ((int)checkTableCmd.ExecuteScalar() > 0) throw new Exception("This table is already occupied! Please go to 'Running Tables' to add items.");
                            }
                        }

                        string orderQuery = @"
                            INSERT INTO Orders (Order_No, Staff_Id, Branch_Id, Customer_Id, Status_Id, Source_Id, Service_Type_Id, 
                                                SubTotal, TaxAmount, DiscountAmount, GrandTotal, Order_Date, Table_Id, Order_Status)
                            OUTPUT INSERTED.Order_Id
                            VALUES (@OrderNo, @StaffId, @BranchId, @CustomerId, 1, @SourceId, @ServiceTypeId, 
                                    @SubTotal, @Tax, @Discount, @GrandTotal, GETDATE(), @TableId, @OrderStatus);";

                        using (SqlCommand cmd = new SqlCommand(orderQuery, con, transaction))
                        {
                            cmd.Parameters.AddWithValue("@OrderNo", generatedOrderNo);
                            cmd.Parameters.AddWithValue("@StaffId", staffId);
                            cmd.Parameters.AddWithValue("@BranchId", branchId);
                            cmd.Parameters.AddWithValue("@CustomerId", (object?)finalCustomerId ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@SourceId", model.Source_Id > 0 ? model.Source_Id : 1);
                            cmd.Parameters.AddWithValue("@ServiceTypeId", model.Service_Type_Id);
                            cmd.Parameters.AddWithValue("@SubTotal", model.SubTotal);
                            cmd.Parameters.AddWithValue("@Tax", model.TaxAmount);
                            cmd.Parameters.AddWithValue("@Discount", model.DiscountAmount);
                            cmd.Parameters.AddWithValue("@GrandTotal", model.GrandTotal);
                            cmd.Parameters.AddWithValue("@TableId", (object?)model.Table_Id ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@OrderStatus", string.IsNullOrEmpty(model.Order_Status) ? "Open" : model.Order_Status);

                            newOrderId = (int)cmd.ExecuteScalar();
                        }

                        string detailQuery = "INSERT INTO Order_Details (Order_Id, Item_Id, Quantity, Unit_Price, Total_Price) VALUES (@OrderId, @ItemId, @Qty, @UnitPrice, @TotalPrice);";

                        foreach (var item in model.Items)
                        {
                            using (SqlCommand detailCmd = new SqlCommand(detailQuery, con, transaction))
                            {
                                detailCmd.Parameters.AddWithValue("@OrderId", newOrderId);
                                detailCmd.Parameters.AddWithValue("@ItemId", item.Item_Id);
                                detailCmd.Parameters.AddWithValue("@Qty", item.Quantity);
                                detailCmd.Parameters.AddWithValue("@UnitPrice", item.Unit_Price);
                                detailCmd.Parameters.AddWithValue("@TotalPrice", item.Total_Price);
                                detailCmd.ExecuteNonQuery();
                            }
                        }

                        if (model.Order_Status == "Paid")
                        {
                            string paymentQuery = "INSERT INTO Payment (Order_Id, Payment_Type_Id, Final_Amount, GST_Amount, Payment_Date) VALUES (@OrderId, @PaymentTypeId, @FinalAmount, @GST, GETDATE());";
                            using (SqlCommand payCmd = new SqlCommand(paymentQuery, con, transaction))
                            {
                                payCmd.Parameters.AddWithValue("@OrderId", newOrderId);
                                payCmd.Parameters.AddWithValue("@PaymentTypeId", model.Payment_Type_Id > 0 ? model.Payment_Type_Id : 1);
                                payCmd.Parameters.AddWithValue("@FinalAmount", model.GrandTotal);
                                payCmd.Parameters.AddWithValue("@GST", model.TaxAmount);
                                payCmd.ExecuteNonQuery();
                            }
                        }

                        transaction.Commit();
                        return newOrderId;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception(ex.Message);
                    }
                }
            }
        }

        public bool UpdateOrder(int orderId, CheckoutViewModel model)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                con.Open();
                using (SqlTransaction transaction = con.BeginTransaction())
                {
                    try
                    {
                        string updateOrderQuery = @"
                            UPDATE Orders 
                            SET SubTotal = @SubTotal, TaxAmount = @Tax, GrandTotal = @GrandTotal, 
                                DiscountAmount = @Discount, Table_Id = @TableId, Order_Status = @OrderStatus
                            WHERE Order_Id = @OrderId";

                        using (SqlCommand cmd = new SqlCommand(updateOrderQuery, con, transaction))
                        {
                            cmd.Parameters.AddWithValue("@OrderId", orderId);
                            cmd.Parameters.AddWithValue("@SubTotal", model.SubTotal);
                            cmd.Parameters.AddWithValue("@Tax", model.TaxAmount);
                            cmd.Parameters.AddWithValue("@Discount", model.DiscountAmount);
                            cmd.Parameters.AddWithValue("@GrandTotal", model.GrandTotal);
                            cmd.Parameters.AddWithValue("@OrderStatus", model.Order_Status);
                            cmd.Parameters.AddWithValue("@TableId", (object?)model.Table_Id ?? DBNull.Value);
                            cmd.ExecuteNonQuery();
                        }

                        string deleteItemsQuery = "DELETE FROM Order_Details WHERE Order_Id = @OrderId";
                        using (SqlCommand delCmd = new SqlCommand(deleteItemsQuery, con, transaction))
                        {
                            delCmd.Parameters.AddWithValue("@OrderId", orderId);
                            delCmd.ExecuteNonQuery();
                        }

                        string detailQuery = "INSERT INTO Order_Details (Order_Id, Item_Id, Quantity, Unit_Price, Total_Price) VALUES (@OrderId, @ItemId, @Qty, @UnitPrice, @TotalPrice);";
                        foreach (var item in model.Items)
                        {
                            using (SqlCommand detailCmd = new SqlCommand(detailQuery, con, transaction))
                            {
                                detailCmd.Parameters.AddWithValue("@OrderId", orderId);
                                detailCmd.Parameters.AddWithValue("@ItemId", item.Item_Id);
                                detailCmd.Parameters.AddWithValue("@Qty", item.Quantity);
                                detailCmd.Parameters.AddWithValue("@UnitPrice", item.Unit_Price);
                                detailCmd.Parameters.AddWithValue("@TotalPrice", item.Total_Price);
                                detailCmd.ExecuteNonQuery();
                            }
                        }

                        if (model.Order_Status == "Paid")
                        {
                            string checkPay = "SELECT COUNT(1) FROM Payment WHERE Order_Id = @OrderId";
                            int payExists = 0;
                            using (SqlCommand chkCmd = new SqlCommand(checkPay, con, transaction))
                            {
                                chkCmd.Parameters.AddWithValue("@OrderId", orderId);
                                payExists = (int)chkCmd.ExecuteScalar();
                            }

                            if (payExists == 0)
                            {
                                string paymentQuery = "INSERT INTO Payment (Order_Id, Payment_Type_Id, Final_Amount, GST_Amount, Payment_Date) VALUES (@OrderId, @PaymentTypeId, @FinalAmount, @GST, GETDATE());";
                                using (SqlCommand payCmd = new SqlCommand(paymentQuery, con, transaction))
                                {
                                    payCmd.Parameters.AddWithValue("@OrderId", orderId);
                                    payCmd.Parameters.AddWithValue("@PaymentTypeId", model.Payment_Type_Id > 0 ? model.Payment_Type_Id : 1);
                                    payCmd.Parameters.AddWithValue("@FinalAmount", model.GrandTotal);
                                    payCmd.Parameters.AddWithValue("@GST", model.TaxAmount);
                                    payCmd.ExecuteNonQuery();
                                }
                            }
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception(ex.Message);
                    }
                }
            }
        }

        public string GetCustomerNameByPhone(string phone)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                string query = "SELECT Customer_Name FROM Customers WHERE Phone = @Phone";
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@Phone", phone);

                con.Open();
                object result = cmd.ExecuteScalar();

                return result != null ? result.ToString() : null;
            }
        }

        public List<OpenOrderViewModel> GetOpenOrders(int branchId)
        {
            List<OpenOrderViewModel> openOrders = new List<OpenOrderViewModel>();
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                string query = @"
                    SELECT o.Order_Id, o.Order_No, o.GrandTotal, o.Order_Date, rt.Table_Number 
                    FROM Orders o 
                    LEFT JOIN Restaurant_Table rt ON o.Table_Id = rt.Table_Id 
                    WHERE o.Order_Status = 'Open' AND o.Branch_Id = @BranchId
                    ORDER BY o.Order_Date DESC";

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.Parameters.AddWithValue("@BranchId", branchId);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    openOrders.Add(new OpenOrderViewModel
                    {
                        Order_Id = Convert.ToInt32(reader["Order_Id"]),
                        Order_No = reader["Order_No"].ToString(),
                        Table_Number = reader["Table_Number"] != DBNull.Value ? reader["Table_Number"].ToString() : "Unknown",
                        GrandTotal = Convert.ToDecimal(reader["GrandTotal"]),
                        Order_Date = Convert.ToDateTime(reader["Order_Date"])
                    });
                }
            }
            return openOrders;
        }

        public CheckoutViewModel GetOrderDetailsById(int orderId)
        {
            CheckoutViewModel model = new CheckoutViewModel();
            model.Items = new List<CartItemViewModel>();

            using (SqlConnection con = _dbHelper.GetConnection())
            {
                string orderQuery = @"
                        SELECT o.*, c.Customer_Name, c.Phone 
                        FROM Orders o
                        LEFT JOIN Customers c ON o.Customer_Id = c.Customer_Id
                        WHERE o.Order_Id = @OrderId";

                using (SqlCommand cmd = new SqlCommand(orderQuery, con))
                {
                    cmd.Parameters.AddWithValue("@OrderId", orderId);
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            model.Customer_Name = reader["Customer_Name"] != DBNull.Value ? reader["Customer_Name"].ToString() : "";
                            model.Customer_Phone = reader["Phone"] != DBNull.Value ? reader["Phone"].ToString() : "";
                            model.Source_Id = Convert.ToInt32(reader["Source_Id"]);
                            model.Service_Type_Id = Convert.ToInt32(reader["Service_Type_Id"]);
                            model.Table_Id = reader["Table_Id"] != DBNull.Value ? Convert.ToInt32(reader["Table_Id"]) : (int?)null;
                            model.Order_Status = reader["Order_Status"].ToString();
                        }
                    }
                }

                string itemQuery = @"
                    SELECT od.*, i.Item_Name, 
                           ISNULL((SELECT TOP 1 GST_Rate FROM Item_GST_Rate WHERE Item_Id = i.Item_Id), 0) AS Gst_Rate
                    FROM Order_Details od
                    INNER JOIN Items i ON od.Item_Id = i.Item_Id
                    WHERE od.Order_Id = @OrderId";

                using (SqlCommand itemCmd = new SqlCommand(itemQuery, con))
                {
                    itemCmd.Parameters.AddWithValue("@OrderId", orderId);
                    using (SqlDataReader reader = itemCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            model.Items.Add(new CartItemViewModel
                            {
                                Item_Id = Convert.ToInt32(reader["Item_Id"]),
                                Item_Name = reader["Item_Name"].ToString(),
                                Quantity = Convert.ToInt32(reader["Quantity"]),
                                Unit_Price = Convert.ToDecimal(reader["Unit_Price"]),
                                Total_Price = Convert.ToDecimal(reader["Total_Price"]),
                                TaxAmount = Convert.ToDecimal(reader["Gst_Rate"])
                            });
                        }
                    }
                }
            }
            return model;
        }

        public List<KitchenOrderViewModel> GetKitchenOrders(int branchId)
        {
            var orders = new List<KitchenOrderViewModel>();
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                string query = @"
                    SELECT o.Order_Id, o.Order_No, o.Order_Date, rt.Table_Number 
                    FROM Orders o 
                    LEFT JOIN Restaurant_Table rt ON o.Table_Id = rt.Table_Id 
                    WHERE o.Order_Status = 'Open' AND o.Branch_Id = @BranchId
                    ORDER BY o.Order_Date ASC";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@BranchId", branchId);
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            orders.Add(new KitchenOrderViewModel
                            {
                                Order_No = reader["Order_No"].ToString(),
                                Table_Number = reader["Table_Number"] != DBNull.Value ? reader["Table_Number"].ToString() : "Takeaway",
                                Order_Date = Convert.ToDateTime(reader["Order_Date"]),
                                Items = new List<KitchenOrderItemViewModel> { new KitchenOrderItemViewModel { Quantity = Convert.ToInt32(reader["Order_Id"]) } }
                            });
                        }
                    }
                }

                foreach (var order in orders)
                {
                    int currentOrderId = order.Items[0].Quantity;
                    order.Items.Clear();

                    string itemQuery = @"
                        SELECT i.Item_Name, od.Quantity 
                        FROM Order_Details od
                        INNER JOIN Items i ON od.Item_Id = i.Item_Id
                        WHERE od.Order_Id = @OrderId";

                    using (SqlCommand itemCmd = new SqlCommand(itemQuery, con))
                    {
                        itemCmd.Parameters.AddWithValue("@OrderId", currentOrderId);
                        using (SqlDataReader itemReader = itemCmd.ExecuteReader())
                        {
                            while (itemReader.Read())
                            {
                                order.Items.Add(new KitchenOrderItemViewModel
                                {
                                    Item_Name = itemReader["Item_Name"].ToString(),
                                    Quantity = Convert.ToInt32(itemReader["Quantity"])
                                });
                            }
                        }
                    }
                }
            }
            return orders;
        }

        public ReceiptViewModel GetReceipt(int orderId)
        {
            var receipt = new ReceiptViewModel();
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                con.Open();
                using (SqlCommand cmd = new SqlCommand("sp_GetReceiptHeader", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@OrderId", orderId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            receipt.Order_Id = Convert.ToInt32(reader["Order_Id"]);
                            receipt.Order_No = reader["Order_No"].ToString();
                            receipt.Order_Date = Convert.ToDateTime(reader["Order_Date"]);
                            receipt.Branch_Name = reader["Branch_Name"].ToString();
                            receipt.Staff_Name = reader["Staff_Name"].ToString();
                            receipt.Customer_Name = reader["Customer_Name"] != DBNull.Value ? reader["Customer_Name"].ToString() : "Walk-in";
                            receipt.Customer_Phone = reader["Phone"] != DBNull.Value ? reader["Phone"].ToString() : "";
                            receipt.Payment_Type = reader["Payment_Type_Desc"].ToString();
                            receipt.SubTotal = Convert.ToDecimal(reader["SubTotal"]);
                            receipt.DiscountAmount = reader["DiscountAmount"] != DBNull.Value ? Convert.ToDecimal(reader["DiscountAmount"]) : 0;
                            receipt.TaxAmount = Convert.ToDecimal(reader["TaxAmount"]);
                            receipt.GrandTotal = Convert.ToDecimal(reader["GrandTotal"]);
                        }
                    }
                }

                using (SqlCommand cmd = new SqlCommand("sp_GetReceiptItems", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@OrderId", orderId);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            receipt.Items.Add(new ReceiptItemViewModel
                            {
                                Item_Name = reader["Item_Name"].ToString(),
                                Quantity = Convert.ToInt32(reader["Quantity"]),
                                Unit_Price = Convert.ToDecimal(reader["Unit_Price"]),
                                Total_Price = Convert.ToDecimal(reader["Total_Price"])
                            });
                        }
                    }
                }
            }
            return receipt;
        }

        public decimal GetTodayActiveDiscount()
        {
            decimal activeDiscount = 0;
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                string query = @"
                    SELECT TOP 1 Offer_Percent 
                    FROM Offers 
                    WHERE Is_Active = 1 
                    AND CAST(GETDATE() AS DATE) >= Start_Date 
                    AND CAST(GETDATE() AS DATE) <= End_Date
                    ORDER BY Offer_Percent DESC";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        activeDiscount = Convert.ToDecimal(result);
                    }
                }
            }
            return activeDiscount;
        }

        public List<RestaurantTable> GetTablesForPos(int branchId)
        {
            List<RestaurantTable> tables = new List<RestaurantTable>();
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetTablesForPos", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BranchId", branchId);
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            tables.Add(new RestaurantTable
                            {
                                Table_Id = Convert.ToInt32(reader["Table_Id"]),
                                Table_Number = reader["Table_Number"].ToString(),
                                Size = Convert.ToInt32(reader["Size"]),
                                Status = reader["Status"].ToString()
                            });
                        }
                    }
                }
            }
            return tables;
        }

        public List<PaymentType> GetActivePaymentTypes()
        {
            List<PaymentType> list = new List<PaymentType>();
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetActivePaymentTypes", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new PaymentType
                            {
                                PT_Id = Convert.ToInt32(reader["PT_Id"]),
                                PT_Desc = reader["PT_Desc"].ToString()
                            });
                        }
                    }
                }
            }
            return list;
        }

        public List<OrderSource> GetActiveOrderSources()
        {
            List<OrderSource> list = new List<OrderSource>();
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("sp_GetActiveOrderSources", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(new OrderSource
                            {
                                Source_Id = Convert.ToInt32(reader["Source_Id"]),
                                Source_Desc = reader["Source_Desc"].ToString()
                            });
                        }
                    }
                }
            }
            return list;
        }

        public List<FeedbackType> GetFeedbackTypesForPos()
        {
            List<FeedbackType> types = new List<FeedbackType>();
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                string query = "SELECT Feedback_Type_Id, Feedback_Type_Name FROM Feedback_Type";
                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            types.Add(new FeedbackType
                            {
                                Feedback_Type_Id = Convert.ToInt32(reader["Feedback_Type_Id"]),
                                Feedback_Type_Name = reader["Feedback_Type_Name"].ToString()
                            });
                        }
                    }
                }
            }
            return types;
        }

        public bool SaveOrderFeedback(int orderId, int feedbackTypeId, int rating, string comments)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("sp_InsertOrderFeedback", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@OrderId", orderId);
                    cmd.Parameters.AddWithValue("@FeedbackTypeId", feedbackTypeId);
                    cmd.Parameters.AddWithValue("@Rating", rating);
                    cmd.Parameters.AddWithValue("@Comments", (object?)comments ?? DBNull.Value);

                    con.Open();
                    int rows = cmd.ExecuteNonQuery();
                    return rows > 0;
                }
            }
        }

        public ManualFeedbackViewModel GetLatestCustomerOrder(string phone)
        {
            var model = new ManualFeedbackViewModel();
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                string query = @"
                    SELECT TOP 1 c.Customer_Name, o.Order_Id 
                    FROM Customers c
                    INNER JOIN Orders o ON c.Customer_Id = o.Customer_Id
                    WHERE c.Phone = @Phone
                    ORDER BY o.Order_Date DESC";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@Phone", phone);
                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            model.Customer_Name = reader["Customer_Name"].ToString();
                            model.Order_Id = Convert.ToInt32(reader["Order_Id"]);
                        }
                    }
                }
            }
            return model;
        }


    }
}