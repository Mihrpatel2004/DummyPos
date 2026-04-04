using DummyPos.Data;
using DummyPos.Models;
using DummyPos.Repositories.Interface;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace DummyPos.Repositories.Implementation
{
    public class TableRepo : ITableRepo
    {
        private readonly DbHelper _dbHelper;
        public TableRepo(DbHelper dbHelper) { _dbHelper = dbHelper; }

        public List<RestaurantTable> GetTablesByBranch(int branchId)
        {
            List<RestaurantTable> list = new List<RestaurantTable>();
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_GetTablesByBranch", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BranchId", branchId);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new RestaurantTable
                    {
                        Table_Id = Convert.ToInt32(reader["Table_Id"]),
                        Branch_Id = Convert.ToInt32(reader["Branch_Id"]),
                        Table_Number = reader["Table_Number"].ToString(),
                        Table_Desc = reader["Table_Desc"].ToString(),
                        Size = Convert.ToInt32(reader["Size"]),
                        Status = reader["Status"].ToString()
                    });
                }
            }
            return list;
        }

        public RestaurantTable GetTableById(int id)
        {
            RestaurantTable table = null;
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_GetTableById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TableId", id);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    table = new RestaurantTable
                    {
                        Table_Id = Convert.ToInt32(reader["Table_Id"]),
                        Branch_Id = Convert.ToInt32(reader["Branch_Id"]),
                        Table_Number = reader["Table_Number"].ToString(),
                        Table_Desc = reader["Table_Desc"].ToString(),
                        Size = Convert.ToInt32(reader["Size"]),
                        Status = reader["Status"].ToString()
                    };
                }
            }
            return table;
        }

        public void AddTable(RestaurantTable table)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_InsertTable", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BranchId", table.Branch_Id);
                cmd.Parameters.AddWithValue("@TableNumber", table.Table_Number);
                cmd.Parameters.AddWithValue("@TableDesc", table.Table_Desc);
                cmd.Parameters.AddWithValue("@Size", table.Size);
                cmd.Parameters.AddWithValue("@Status", table.Status);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateTable(RestaurantTable table)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateTable", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TableId", table.Table_Id);
                cmd.Parameters.AddWithValue("@BranchId", table.Branch_Id);
                cmd.Parameters.AddWithValue("@TableNumber", table.Table_Number);
                cmd.Parameters.AddWithValue("@TableDesc", table.Table_Desc);
                cmd.Parameters.AddWithValue("@Size", table.Size);
                cmd.Parameters.AddWithValue("@Status", table.Status);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteTable(int id)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_DeleteTable", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@TableId", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}