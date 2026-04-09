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
    public class ItemRepo : IItemRepo
    {
        private readonly DbHelper _dbHelper;

        public ItemRepo(DbHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }
        

        public List<Item> GetAllItems()
        {
            List<Item> list = new List<Item>();
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_GetAllItems", con);
                cmd.CommandType = CommandType.StoredProcedure;
               /* cmd.CommandTimeout = 120;*/
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new Item
                    {
                        Item_Id = Convert.ToInt32(reader["Item_Id"]),
                        Item_Name = reader["Item_Name"].ToString(),
                        Amount = Convert.ToDecimal(reader["Amount"]),
                        Item_Category_Id = Convert.ToInt32(reader["Item_Category_Id"]),
                        Prepare_Time_Minutes = reader["Prepare_Time_Minutes"] != DBNull.Value ? Convert.ToInt32(reader["Prepare_Time_Minutes"]) : (int?)null,
                        Is_Active = Convert.ToBoolean(reader["Is_Active"]),
                        // From the JOIN in sp_GetAllItems
                        Item_Category_Desc = reader["Item_Category_Desc"].ToString(),
                        // Handling the VARBINARY(MAX) image safely
                        Item_Image = reader["Item_Image"] != DBNull.Value ? (byte[])reader["Item_Image"] : null
                    });
                }
            }
            return list;
        }

        public Item GetItemById(int id)
        {
            Item item = null;
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_GetItemById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    item = new Item
                    {
                        Item_Id = Convert.ToInt32(reader["Item_Id"]),
                        Item_Name = reader["Item_Name"].ToString(),
                        Amount = Convert.ToDecimal(reader["Amount"]),
                        Item_Category_Id = Convert.ToInt32(reader["Item_Category_Id"]),
                        Prepare_Time_Minutes = reader["Prepare_Time_Minutes"] != DBNull.Value ? Convert.ToInt32(reader["Prepare_Time_Minutes"]) : (int?)null,
                        Is_Active = Convert.ToBoolean(reader["Is_Active"]),
                        Item_Image = reader["Item_Image"] != DBNull.Value ? (byte[])reader["Item_Image"] : null,

                        // ADD THIS LINE RIGHT HERE:
                        Item_Category_Desc = reader["Item_Category_Desc"].ToString()
                    };
                }
            }
            return item;
        }
        public void AddItem(Item item)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_InsertItem", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", item.Item_Name);
                cmd.Parameters.AddWithValue("@Amount", item.Amount);
                cmd.Parameters.AddWithValue("@CategoryId", item.Item_Category_Id);

                // Handle nullable Prepare Time
                cmd.Parameters.AddWithValue("@PrepareTime", (object?)item.Prepare_Time_Minutes ?? DBNull.Value);

                // Handle optional Image upload (byte[])
                cmd.Parameters.AddWithValue("@Image", (object?)item.Item_Image ?? DBNull.Value);

                cmd.Parameters.AddWithValue("@IsActive", item.Is_Active);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateItem(Item item)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateItem", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", item.Item_Id);
                cmd.Parameters.AddWithValue("@Name", item.Item_Name);
                cmd.Parameters.AddWithValue("@Amount", item.Amount);
                cmd.Parameters.AddWithValue("@CategoryId", item.Item_Category_Id);
                cmd.Parameters.AddWithValue("@PrepareTime", (object?)item.Prepare_Time_Minutes ?? DBNull.Value);

                // FIX: Explicitly tell SQL this is a VarBinary(MAX) so it doesn't default to a string!
                SqlParameter imageParam = new SqlParameter("@Image", SqlDbType.VarBinary, -1);
                imageParam.Value = (object?)item.Item_Image ?? DBNull.Value;
                cmd.Parameters.Add(imageParam);

                cmd.Parameters.AddWithValue("@IsActive", item.Is_Active);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteItem(int id)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_DeleteItem", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void ActivateItem(int id)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_ActivateItem", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeactivateItem(int id)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_DeactivateItem", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Populates the Category dropdown correctly
        public List<SelectListItem> GetCategoryDropdown()
        {
            List<SelectListItem> categories = new List<SelectListItem>();
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_GetActiveCategoriesForDropdown", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    categories.Add(new SelectListItem
                    {
                        Value = reader["Item_Category_Id"].ToString(),
                        Text = reader["Item_Category_Desc"].ToString()
                    });
                }
            }
            return categories;
        }

        //
        public List<ItemSearchViewModel> SearchAdminItems(string searchTerm)
        {
            List<ItemSearchViewModel> itemList = new List<ItemSearchViewModel>();

            using (SqlConnection con = _dbHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("sp_SearchItems_Universal", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@SearchTerm", string.IsNullOrEmpty(searchTerm) ? DBNull.Value : (object)searchTerm);

                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var item = new ItemSearchViewModel
                            {
                                Item_Id = Convert.ToInt32(reader["Item_Id"]),
                                Item_Name = reader["Item_Name"].ToString(),
                                Amount = Convert.ToDecimal(reader["Amount"]),

                                // New Admin Fields
                                Category = reader["Category"].ToString(),
                                Prepare_Time_Minutes = reader["Prepare_Time_Minutes"] != DBNull.Value ? Convert.ToInt32(reader["Prepare_Time_Minutes"]) : 0,
                                Is_Active = Convert.ToBoolean(reader["Is_Active"])
                            };

                            // Format the byte[] into your ImageDataUrl string safely
                            if (reader["Item_Image"] != DBNull.Value)
                            {
                                byte[] imgBytes = (byte[])reader["Item_Image"];
                                string base64String = Convert.ToBase64String(imgBytes);
                                item.ImageDataUrl = $"data:image/jpeg;base64,{base64String}";
                            }

                            itemList.Add(item);
                        }
                    }
                }
            }
            return itemList;
        }
        //
    }
}