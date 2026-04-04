using DummyPos.Data;
using DummyPos.Models;
using DummyPos.Repositories.Interface;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace DummyPos.Repositories.Implementation
{
    public class OrderSourceRepo : IOrderSourceRepo
    {
        private readonly DbHelper _dbHelper;

        public OrderSourceRepo(DbHelper dbHelper) => _dbHelper = dbHelper;

        public List<OrderSource> GetAllOrderSources()
        {
            var list = new List<OrderSource>();
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_GetAllOrderSources", con) { CommandType = CommandType.StoredProcedure };
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new OrderSource
                    {
                        Source_Id = Convert.ToInt32(reader["Source_Id"]),
                        Source_Desc = reader["Source_Desc"].ToString(),
                        Is_Active = Convert.ToBoolean(reader["Is_Active"])
                    });
                }
            }
            return list;
        }

        public OrderSource GetOrderSourceById(int id)
        {
            OrderSource source = null;
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_GetOrderSourceById", con) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    source = new OrderSource
                    {
                        Source_Id = Convert.ToInt32(reader["Source_Id"]),
                        Source_Desc = reader["Source_Desc"].ToString(),
                        Is_Active = Convert.ToBoolean(reader["Is_Active"])
                    };
                }
            }
            return source;
        }

        public void AddOrderSource(OrderSource orderSource)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_InsertOrderSource", con) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@Desc", orderSource.Source_Desc);
                cmd.Parameters.AddWithValue("@IsActive", orderSource.Is_Active);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateOrderSource(OrderSource orderSource)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateOrderSource", con) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@Id", orderSource.Source_Id);
                cmd.Parameters.AddWithValue("@Desc", orderSource.Source_Desc);
                cmd.Parameters.AddWithValue("@IsActive", orderSource.Is_Active);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteOrderSource(int id)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_DeleteOrderSource", con) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void ActivateOrderSource(int id)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_ActivateOrderSource", con) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeactivateOrderSource(int id)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_DeactivateOrderSource", con) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}