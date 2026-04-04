using DummyPos.Data;
using DummyPos.Models;
using DummyPos.Repositories.Interface;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace DummyPos.Repositories.Implementation
{
    public class OrderStatusRepo : IOrderStatusRepo
    {
        private readonly DbHelper _dbHelper;

        public OrderStatusRepo(DbHelper dbHelper) => _dbHelper = dbHelper;

        public List<OrderStatus> GetAllOrderStatuses()
        {
            var list = new List<OrderStatus>();
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_GetAllOrderStatuses", con) { CommandType = CommandType.StoredProcedure };
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new OrderStatus
                    {
                        Status_Id = Convert.ToInt32(reader["Status_Id"]),
                        Status_Desc = reader["Status_Desc"].ToString(),
                        Is_Active = Convert.ToBoolean(reader["Is_Active"])
                    });
                }
            }
            return list;
        }

        public OrderStatus GetOrderStatusById(int id)
        {
            OrderStatus status = null;
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_GetOrderStatusById", con) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    status = new OrderStatus
                    {
                        Status_Id = Convert.ToInt32(reader["Status_Id"]),
                        Status_Desc = reader["Status_Desc"].ToString(),
                        Is_Active = Convert.ToBoolean(reader["Is_Active"])
                    };
                }
            }
            return status;
        }

        public void AddOrderStatus(OrderStatus orderStatus)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_InsertOrderStatus", con) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@Desc", orderStatus.Status_Desc);
                cmd.Parameters.AddWithValue("@IsActive", orderStatus.Is_Active);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateOrderStatus(OrderStatus orderStatus)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateOrderStatus", con) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@Id", orderStatus.Status_Id);
                cmd.Parameters.AddWithValue("@Desc", orderStatus.Status_Desc);
                cmd.Parameters.AddWithValue("@IsActive", orderStatus.Is_Active);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteOrderStatus(int id)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_DeleteOrderStatus", con) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void ActivateOrderStatus(int id)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_ActivateOrderStatus", con) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeactivateOrderStatus(int id)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_DeactivateOrderStatus", con) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}