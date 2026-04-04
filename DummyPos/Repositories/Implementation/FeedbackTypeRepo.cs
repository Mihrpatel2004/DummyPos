using DummyPos.Data;
using DummyPos.Models;
using DummyPos.Repositories.Interface;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace DummyPos.Repositories.Implementation
{
    public class FeedbackTypeRepo : IFeedbackTypeRepo
    {
        private readonly DbHelper _dbHelper;

        public FeedbackTypeRepo(DbHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public List<FeedbackType> GetAllFeedbackTypes()
        {
            List<FeedbackType> list = new List<FeedbackType>();
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_GetAllFeedbackTypes", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new FeedbackType
                    {
                        Feedback_Type_Id = Convert.ToInt32(reader["Feedback_Type_Id"]),
                        Feedback_Type_Name = reader["Feedback_Type_Name"].ToString()
                    });
                }
            }
            return list;
        }

        public FeedbackType GetFeedbackTypeById(int id)
        {
            FeedbackType feedbackType = null;
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_GetFeedbackTypeById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    feedbackType = new FeedbackType
                    {
                        Feedback_Type_Id = Convert.ToInt32(reader["Feedback_Type_Id"]),
                        Feedback_Type_Name = reader["Feedback_Type_Name"].ToString()
                    };
                }
            }
            return feedbackType;
        }

        public void AddFeedbackType(FeedbackType feedbackType)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_InsertFeedbackType", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", feedbackType.Feedback_Type_Name);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateFeedbackType(FeedbackType feedbackType)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateFeedbackType", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", feedbackType.Feedback_Type_Id);
                cmd.Parameters.AddWithValue("@Name", feedbackType.Feedback_Type_Name);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteFeedbackType(int id)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_DeleteFeedbackType", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}