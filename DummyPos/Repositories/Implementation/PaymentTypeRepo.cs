using DummyPos.Data;
using DummyPos.Models;
using DummyPos.Repositories.Interface;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace DummyPos.Repositories.Implementation
{
    public class PaymentTypeRepo : IPaymentTypeRepo
    {
        private readonly DbHelper _dbHelper;

        public PaymentTypeRepo(DbHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public List<PaymentType> GetAllPaymentTypes()
        {
            List<PaymentType> list = new List<PaymentType>();
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_GetAllPaymentTypes", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new PaymentType
                    {
                        PT_Id = Convert.ToInt32(reader["PT_Id"]),
                        PT_Desc = reader["PT_Desc"].ToString(),
                        Is_Active = Convert.ToBoolean(reader["Is_Active"])
                    });
                }
            }
            return list;
        }

        public PaymentType GetPaymentTypeById(int id)
        {
            PaymentType paymentType = null;
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_GetPaymentTypeById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    paymentType = new PaymentType
                    {
                        PT_Id = Convert.ToInt32(reader["PT_Id"]),
                        PT_Desc = reader["PT_Desc"].ToString(),
                        Is_Active = Convert.ToBoolean(reader["Is_Active"])
                    };
                }
            }
            return paymentType;
        }

        public void AddPaymentType(PaymentType paymentType)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_InsertPaymentType", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Desc", paymentType.PT_Desc);
                cmd.Parameters.AddWithValue("@IsActive", paymentType.Is_Active);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdatePaymentType(PaymentType paymentType)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_UpdatePaymentType", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", paymentType.PT_Id);
                cmd.Parameters.AddWithValue("@Desc", paymentType.PT_Desc);
                cmd.Parameters.AddWithValue("@IsActive", paymentType.Is_Active);
                con.Open();
                cmd.ExecuteNonQuery();
              
            }
        }

        public void DeletePaymentType(int id)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_DeletePaymentType", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void ActivatePaymentType(int id)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_ActivatePaymentType", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeactivatePaymentType(int id)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_DeactivatePaymentType", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}