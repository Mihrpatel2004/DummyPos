using DummyPos.Data;
using DummyPos.Models;
using DummyPos.Repositories.Interface;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace DummyPos.Repositories.Implementation
{
    public class ItemGstRateRepo : IItemGstRateRepo
    {
        private readonly DbHelper _dbHelper;

        public ItemGstRateRepo(DbHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public List<ItemGstRate> GetAllItemGstRates()
        {
            var list = new List<ItemGstRate>();
            using (var con = _dbHelper.GetConnection())
            {
                var cmd = new SqlCommand("sp_GetAllItemGstRates", con) { CommandType = CommandType.StoredProcedure };
                con.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new ItemGstRate
                    {
                        IGR_Id = Convert.ToInt32(reader["IGR_Id"]),
                        Item_Id = Convert.ToInt32(reader["Item_Id"]),
                        Service_Type_Id = Convert.ToInt32(reader["Service_Type_Id"]),
                        GST_Rate = Convert.ToDecimal(reader["GST_Rate"]),
                        Item_Name = reader["Item_Name"].ToString(),
                        Service_Type_Desc = reader["Service_Type_Desc"].ToString()
                    });
                }
            }
            return list;
        }

        public ItemGstRate GetItemGstRateById(int id)
        {
            ItemGstRate rate = null;
            using (var con = _dbHelper.GetConnection())
            {
                var cmd = new SqlCommand("sp_GetItemGstRateById", con) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    rate = new ItemGstRate
                    {
                        IGR_Id = Convert.ToInt32(reader["IGR_Id"]),
                        Item_Id = Convert.ToInt32(reader["Item_Id"]),
                        Service_Type_Id = Convert.ToInt32(reader["Service_Type_Id"]),
                        GST_Rate = Convert.ToDecimal(reader["GST_Rate"]),
                        Item_Name = reader["Item_Name"].ToString(),
                        Service_Type_Desc = reader["Service_Type_Desc"].ToString()
                    };
                }
            }
            return rate;
        }

        public void AddItemGstRate(ItemGstRate itemGstRate)
        {
            using (var con = _dbHelper.GetConnection())
            {
                var cmd = new SqlCommand("sp_InsertItemGstRate", con) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@ItemId", itemGstRate.Item_Id);
                cmd.Parameters.AddWithValue("@ServiceTypeId", itemGstRate.Service_Type_Id);
                cmd.Parameters.AddWithValue("@GstRate", itemGstRate.GST_Rate);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateItemGstRate(ItemGstRate itemGstRate)
        {
            using (var con = _dbHelper.GetConnection())
            {
                var cmd = new SqlCommand("sp_UpdateItemGstRate", con) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@Id", itemGstRate.IGR_Id);
                cmd.Parameters.AddWithValue("@ItemId", itemGstRate.Item_Id);
                cmd.Parameters.AddWithValue("@ServiceTypeId", itemGstRate.Service_Type_Id);
                cmd.Parameters.AddWithValue("@GstRate", itemGstRate.GST_Rate);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteItemGstRate(int id)
        {
            using (var con = _dbHelper.GetConnection())
            {
                var cmd = new SqlCommand("sp_DeleteItemGstRate", con) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public List<SelectListItem> GetItemsDropdown()
        {
            var list = new List<SelectListItem>();
            using (var con = _dbHelper.GetConnection())
            {
                var cmd = new SqlCommand("sp_GetActiveItemsForDropdown", con) { CommandType = CommandType.StoredProcedure };
                con.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new SelectListItem { Value = reader["Item_Id"].ToString(), Text = reader["Item_Name"].ToString() });
                }
            }
            return list;
        }

        public List<SelectListItem> GetServiceTypesDropdown()
        {
            var list = new List<SelectListItem>();
            using (var con = _dbHelper.GetConnection())
            {
                var cmd = new SqlCommand("sp_GetActiveServiceTypesForDropdown", con) { CommandType = CommandType.StoredProcedure };
                con.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new SelectListItem { Value = reader["Service_Type_Id"].ToString(), Text = reader["Service_Type_Desc"].ToString() });
                }
            }
            return list;
        }
    }
}