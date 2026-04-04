using DummyPos.Data;
using DummyPos.Models;
using DummyPos.Repositories.Interface;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace DummyPos.Repositories.Implementation
{
    public class ToppingRepo : IToppingRepo
    {
        private readonly DbHelper _dbHelper;

        public ToppingRepo(DbHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public List<Toppings> GetAllToppings()
        {
            List<Toppings> list = new List<Toppings>();
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_GetAllToppings", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new Toppings
                    {
                        Topping_Id = Convert.ToInt32(reader["Topping_Id"]),
                        Topping_Name = reader["Topping_Name"].ToString(),
                        Price = Convert.ToDecimal(reader["Price"]),
                        Is_Active = Convert.ToBoolean(reader["Is_Active"])
                    });
                }
            }
            return list;
        }

        public Toppings GetToppingById(int id)
        {
            Toppings topping = null;
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_GetToppingById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    topping = new Toppings
                    {
                        Topping_Id = Convert.ToInt32(reader["Topping_Id"]),
                        Topping_Name = reader["Topping_Name"].ToString(),
                        Price = Convert.ToDecimal(reader["Price"]),
                        Is_Active = Convert.ToBoolean(reader["Is_Active"])
                    };
                }
            }
            return topping;
        }

        public void AddTopping(Toppings topping)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_InsertTopping", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", topping.Topping_Name);
                cmd.Parameters.AddWithValue("@Price", topping.Price);
                cmd.Parameters.AddWithValue("@IsActive", topping.Is_Active);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateTopping(Toppings topping)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateTopping", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", topping.Topping_Id);
                cmd.Parameters.AddWithValue("@Name", topping.Topping_Name);
                cmd.Parameters.AddWithValue("@Price", topping.Price);
                cmd.Parameters.AddWithValue("@IsActive", topping.Is_Active);
                con.Open();
                cmd.ExecuteNonQuery();
                
            }
        }

        public void DeleteTopping(int id)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_DeleteTopping", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void ActivateTopping(int id)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_ActivateTopping", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeactivateTopping(int id)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_DeactivateTopping", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}