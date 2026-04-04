using DummyPos.Data;
using DummyPos.Models;
using DummyPos.Repositories.Interface;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DummyPos.Repositories.Implementation
{
    
    public class AdminRepo : IAdminRepo
    {
        private readonly DbHelper _dbHelper;

        public AdminRepo(DbHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }
        public List<Branch> GetAllBranches()
        {
            List<Branch> branches = new List<Branch>();
            using (Microsoft.Data.SqlClient.SqlConnection con = _dbHelper.GetConnection())
            {
                using (Microsoft.Data.SqlClient.SqlCommand cmd = new Microsoft.Data.SqlClient.SqlCommand("SELECT Branch_Id, Branch_Name, Is_Active FROM Branch", con))
                {
                    con.Open();
                    using (Microsoft.Data.SqlClient.SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            branches.Add(new Branch
                            {
                                Branch_Id = Convert.ToInt32(reader["Branch_Id"]),
                                Branch_Name = reader["Branch_Name"].ToString(),
                                Is_Active = Convert.ToBoolean(reader["Is_Active"])
                            });
                        }
                    }
                }
            }
            return branches;
        }

        public List<StaffDetailViewModel> GetStaffByBranchId(int branchId)
        {
            List<StaffDetailViewModel> staffList = new List<StaffDetailViewModel>();
            using (Microsoft.Data.SqlClient.SqlConnection con = _dbHelper.GetConnection())
            {
                string query = @"
                    SELECT s.Staff_Id, s.Staff_Name, r.Role_Desc, s.Phone, s.Email, s.Is_Active, b.Branch_Name 
                    FROM Staffs s
                    INNER JOIN Role r ON s.Role_Id = r.Role_Id
                    INNER JOIN Branch b ON s.BranchId = b.Branch_Id
                    WHERE s.BranchId = @BranchId";

                using (Microsoft.Data.SqlClient.SqlCommand cmd = new Microsoft.Data.SqlClient.SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@BranchId", branchId);
                    con.Open();
                    using (Microsoft.Data.SqlClient.SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            staffList.Add(new StaffDetailViewModel
                            {
                                Staff_Id = Convert.ToInt32(reader["Staff_Id"]),
                                Staff_Name = reader["Staff_Name"].ToString(),
                                Role_Desc = reader["Role_Desc"].ToString(),
                                Phone = reader["Phone"].ToString(),
                                Email = reader["Email"].ToString(),
                                Branch_Name = reader["Branch_Name"].ToString(),
                                Is_Active = Convert.ToBoolean(reader["Is_Active"])
                            });
                        }
                    }
                }
            }
            return staffList;
        }

        public StaffDetailViewModel GetStaffDetailsById(int staffId)
        {
            var staff = new StaffDetailViewModel();
            using (Microsoft.Data.SqlClient.SqlConnection con = _dbHelper.GetConnection())
            {
                string query = @"
                    SELECT s.*, r.Role_Desc, b.Branch_Name 
                    FROM Staffs s
                    INNER JOIN Role r ON s.Role_Id = r.Role_Id
                    INNER JOIN Branch b ON s.BranchId = b.Branch_Id
                    WHERE s.Staff_Id = @StaffId";

                using (Microsoft.Data.SqlClient.SqlCommand cmd = new Microsoft.Data.SqlClient.SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@StaffId", staffId);
                    con.Open();
                    using (Microsoft.Data.SqlClient.SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            staff.Staff_Id = Convert.ToInt32(reader["Staff_Id"]);
                            staff.Staff_Name = reader["Staff_Name"].ToString();
                            staff.Role_Desc = reader["Role_Desc"].ToString();
                            staff.Phone = reader["Phone"] != DBNull.Value ? reader["Phone"].ToString() : "N/A";
                            staff.Email = reader["Email"] != DBNull.Value ? reader["Email"].ToString() : "N/A";
                            staff.Hire_Date = reader["Hire_Date"] != DBNull.Value ? Convert.ToDateTime(reader["Hire_Date"]) : (DateTime?)null;
                            staff.Salary = reader["Salary"] != DBNull.Value ? Convert.ToDecimal(reader["Salary"]) : (decimal?)null;
                            staff.Branch_Name = reader["Branch_Name"].ToString();
                            staff.Username = reader["Username"] != DBNull.Value ? reader["Username"].ToString() : "N/A";
                            staff.Last_Login = reader["Last_Login"] != DBNull.Value ? Convert.ToDateTime(reader["Last_Login"]) : (DateTime?)null;
                            staff.Is_Active = Convert.ToBoolean(reader["Is_Active"]);
                        }
                    }
                }
            }
            return staff;
        }
        public DashboardViewModel GetDashboardMetrics(int? branchId)
        {
            var model = new DashboardViewModel();
            using (Microsoft.Data.SqlClient.SqlConnection con = _dbHelper.GetConnection())
            {
                con.Open();

                // 1. Get KPI Totals for TODAY
                string kpiQuery = @"
                    SELECT 
                        ISNULL(SUM(GrandTotal), 0) AS TodayRevenue,
                        COUNT(Order_Id) AS TodayOrders
                    FROM Orders 
                    WHERE CAST(Order_Date AS DATE) = CAST(GETDATE() AS DATE) 
                    AND Order_Status = 'Paid'
                    AND (@BranchId IS NULL OR Branch_Id = @BranchId)";

                using (Microsoft.Data.SqlClient.SqlCommand cmd = new Microsoft.Data.SqlClient.SqlCommand(kpiQuery, con))
                {
                    cmd.Parameters.AddWithValue("@BranchId", (object?)branchId ?? DBNull.Value);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            model.TodayRevenue = Convert.ToDecimal(reader["TodayRevenue"]);
                            model.TodayOrders = Convert.ToInt32(reader["TodayOrders"]);
                        }
                    }
                }

                // 2. Get Average Rating & Active Tables
                using (Microsoft.Data.SqlClient.SqlCommand cmd = new Microsoft.Data.SqlClient.SqlCommand("SELECT ISNULL(AVG(CAST(Rating AS FLOAT)), 0) FROM Feedbacks", con))
                {
                    model.AverageRating = Math.Round(Convert.ToDouble(cmd.ExecuteScalar()), 1);
                }

                string tableQuery = "SELECT COUNT(1) FROM Orders WHERE Order_Status = 'Open' AND (@BranchId IS NULL OR Branch_Id = @BranchId)";
                using (Microsoft.Data.SqlClient.SqlCommand cmd = new Microsoft.Data.SqlClient.SqlCommand(tableQuery, con))
                {
                    cmd.Parameters.AddWithValue("@BranchId", (object?)branchId ?? DBNull.Value);
                    model.ActiveTables = Convert.ToInt32(cmd.ExecuteScalar());
                }

                // 3. Get Sales By Branch (For the Pie Chart)
                string branchSalesQuery = @"
                    SELECT b.Branch_Name, ISNULL(SUM(o.GrandTotal), 0) as TotalSales
                    FROM Branch b
                    LEFT JOIN Orders o ON b.Branch_Id = o.Branch_Id AND o.Order_Status = 'Paid'
                    GROUP BY b.Branch_Name";
                using (Microsoft.Data.SqlClient.SqlCommand cmd = new Microsoft.Data.SqlClient.SqlCommand(branchSalesQuery, con))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            model.SalesByBranch.Add(new ChartData { Label = reader["Branch_Name"].ToString(), Value = Convert.ToDecimal(reader["TotalSales"]) });
                        }
                    }
                }

                // 4. Get Top Selling Items (For the Bar Chart)
                string topItemsQuery = @"
                    SELECT TOP 5 i.Item_Name, SUM(od.Quantity) as QtySold
                    FROM Order_Details od
                    INNER JOIN Items i ON od.Item_Id = i.Item_Id
                    INNER JOIN Orders o ON od.Order_Id = o.Order_Id
                    WHERE o.Order_Status = 'Paid' AND (@BranchId IS NULL OR o.Branch_Id = @BranchId)
                    GROUP BY i.Item_Name
                    ORDER BY QtySold DESC";
                using (Microsoft.Data.SqlClient.SqlCommand cmd = new Microsoft.Data.SqlClient.SqlCommand(topItemsQuery, con))
                {
                    cmd.Parameters.AddWithValue("@BranchId", (object?)branchId ?? DBNull.Value);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            model.TopSellingItems.Add(new ChartData { Label = reader["Item_Name"].ToString(), Value = Convert.ToDecimal(reader["QtySold"]) });
                        }
                    }
                }
            }
            return model;
        }

        public List<AdminOrderViewModel> GetFilteredOrders(int branchId, DateTime? startDate, DateTime? endDate)
        {
            var list = new List<AdminOrderViewModel>();
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("SP_GetAdminFilteredOrders", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@BranchId", branchId);
                cmd.Parameters.AddWithValue("@StartDate", startDate ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@EndDate", endDate ?? (object)DBNull.Value);

                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new AdminOrderViewModel
                        {
                            Order_Id = Convert.ToInt32(reader["Order_Id"]),
                            Order_No = reader["Order_No"].ToString(),
                            Customer_Name = reader["Customer_Name"].ToString(),
                            Staff_Name = reader["Staff_Name"].ToString(),
                            Branch_Name = reader["Branch_Name"].ToString(),
                            Order_Date = Convert.ToDateTime(reader["Order_Date"]),
                            GrandTotal = Convert.ToDecimal(reader["GrandTotal"]),
                            Status_Name = reader["Status_Name"].ToString()
                        });
                    }
                }
            }
            return list;
        }

        public List<AdminOrderItemViewModel> GetOrderItemsAdmin(int orderId)
        {
            var list = new List<AdminOrderItemViewModel>();
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("SP_GetAdminOrderDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OrderId", orderId);

                con.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new AdminOrderItemViewModel
                        {
                            Item_Name = reader["Item_Name"].ToString(),
                            Quantity = Convert.ToInt32(reader["Quantity"]),
                            Total_Price = Convert.ToDecimal(reader["Total_Price"])
                        });
                    }
                }
            }
            return list;
        }
    }
}
