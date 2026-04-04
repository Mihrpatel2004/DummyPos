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
    public class ScreenRepo : IScreenRepo
    {
        private readonly DbHelper _dbHelper;
        public ScreenRepo(DbHelper dbHelper) => _dbHelper = dbHelper;

        public List<ItemDisplayScreen> GetScreensByStationId(int stationId)
        {
            var list = new List<ItemDisplayScreen>();
            using (var con = _dbHelper.GetConnection())
            {
                var cmd = new SqlCommand("sp_SCR_GetByStationId", con) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@StationId", stationId);
                con.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new ItemDisplayScreen
                    {
                        Screen_No = Convert.ToInt32(reader["Screen_No"]),
                        Station_Id = Convert.ToInt32(reader["Station_Id"]),
                        Item_Id = Convert.ToInt32(reader["Item_Id"]),
                        Is_Active = Convert.ToBoolean(reader["Is_Active"]),
                        Item_Name = reader["Item_Name"].ToString()
                    });
                }
            }
            return list;
        }

        public ItemDisplayScreen GetScreenById(int id)
        {
            ItemDisplayScreen screen = null;
            using (var con = _dbHelper.GetConnection())
            {
                var cmd = new SqlCommand("sp_SCR_GetById", con) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    screen = new ItemDisplayScreen
                    {
                        Screen_No = Convert.ToInt32(reader["Screen_No"]),
                        Station_Id = Convert.ToInt32(reader["Station_Id"]),
                        Item_Id = Convert.ToInt32(reader["Item_Id"]),
                        Is_Active = Convert.ToBoolean(reader["Is_Active"])
                    };
                }
            }
            return screen;
        }

        public void AddScreen(ItemDisplayScreen screen)
        {
            using (var con = _dbHelper.GetConnection())
            {
                var cmd = new SqlCommand("sp_SCR_Insert", con) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@StationId", screen.Station_Id);
                cmd.Parameters.AddWithValue("@ItemId", screen.Item_Id);
                cmd.Parameters.AddWithValue("@IsActive", screen.Is_Active);
                con.Open(); cmd.ExecuteNonQuery();
            }
        }

        public void UpdateScreen(ItemDisplayScreen screen)
        {
            using (var con = _dbHelper.GetConnection())
            {
                var cmd = new SqlCommand("sp_SCR_Update", con) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@Id", screen.Screen_No);
                cmd.Parameters.AddWithValue("@ItemId", screen.Item_Id);
                cmd.Parameters.AddWithValue("@IsActive", screen.Is_Active);
                con.Open(); cmd.ExecuteNonQuery();
            }
        }

        public void DeleteScreen(int id)
        {
            using (var con = _dbHelper.GetConnection())
            {
                var cmd = new SqlCommand("sp_SCR_Delete", con) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open(); cmd.ExecuteNonQuery();
            }
        }

      /*  public void ToggleActive(int id, bool status)
        {
            using (var con = _dbHelper.GetConnection())
            {
                var cmd = new SqlCommand("sp_SCR_ToggleActive", con) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.Parameters.AddWithValue("@Status", status);
                con.Open(); cmd.ExecuteNonQuery();
            }
        }*/

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
                    list.Add(new SelectListItem
                    {
                        Value = reader["Item_Id"].ToString(),
                        Text = reader["Item_Name"].ToString()
                    });
                }
            }
            return list;
        }
       /* public void ActivateK_S(int id)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                // Make sure you have a stored procedure named 'sp_ActivateKitchenStation' in SQL
                SqlCommand cmd = new SqlCommand("sp_ks_Active", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeactivateK_S(int id)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                // Make sure you have a stored procedure named 'sp_DeactivateKitchenStation' in SQL
                SqlCommand cmd = new SqlCommand("sp_ks_Deactive", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }*/
        public void DeactivateScreen(int id)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                // Make sure you have a stored procedure named 'sp_DeactivateKitchenStation' in SQL
                SqlCommand cmd = new SqlCommand("sp_ks_Deactive", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
           
        }

        public void ActivateScreen(int id)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                // Make sure you have a stored procedure named 'sp_ActivateKitchenStation' in SQL
                SqlCommand cmd = new SqlCommand("sp_ks_Active", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
            
        }
    }
}