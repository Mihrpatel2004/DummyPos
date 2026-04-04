using DummyPos.Data;
using DummyPos.Models;
using DummyPos.Repositories.Interface;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace DummyPos.Repositories.Implementation
{
    public class ItemCategoryRepo : IItemCategoryRepo
    {
        private readonly DbHelper _dbHelper;

        public ItemCategoryRepo(DbHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public List<ItemCategory> GetAllItemCategories()
        {
            List<ItemCategory> list = new List<ItemCategory>();
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_GetAllItemCategories", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new ItemCategory
                    {
                        Item_Category_Id = Convert.ToInt32(reader["Item_Category_Id"]),
                        Item_Category_Desc = reader["Item_Category_Desc"].ToString(),
                        Is_Active = Convert.ToBoolean(reader["Is_Active"])
                    });
                }
            }
            return list;
        }

        public ItemCategory GetItemCategoryById(int id)
        {
            ItemCategory category = null;
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_GetItemCategoryById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    category = new ItemCategory
                    {
                        Item_Category_Id = Convert.ToInt32(reader["Item_Category_Id"]),
                        Item_Category_Desc = reader["Item_Category_Desc"].ToString(),
                        Is_Active = Convert.ToBoolean(reader["Is_Active"])
                    };
                }
            }
            return category;
        }

        public void AddItemCategory(ItemCategory category)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_InsertItemCategory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Desc", category.Item_Category_Desc);
                cmd.Parameters.AddWithValue("@IsActive", category.Is_Active);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateItemCategory(ItemCategory category)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateItemCategory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", category.Item_Category_Id);
                cmd.Parameters.AddWithValue("@Desc", category.Item_Category_Desc);
                cmd.Parameters.AddWithValue("@IsActive", category.Is_Active);
               
                con.Open();
                cmd.ExecuteNonQuery();
             
            }
        }

        public void DeleteItemCategory(int id)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_DeleteItemCategory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void ActivateItemCategory(int id)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_ActivateItemCategory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeactivateItemCategory(int id)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_DeactivateItemCategory", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}