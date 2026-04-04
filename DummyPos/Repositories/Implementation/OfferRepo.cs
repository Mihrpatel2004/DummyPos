using DummyPos.Data;
using DummyPos.Models;
using DummyPos.Repositories.Interface;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace DummyPos.Repositories.Implementation
{
    public class OfferRepo : IOfferRepo
    {
        private readonly DbHelper _dbHelper;
        public OfferRepo(DbHelper dbHelper) { _dbHelper = dbHelper; }

        public List<Offer> GetAllOffers()
        {
            List<Offer> list = new List<Offer>();
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_GetAllOffersData", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new Offer
                    {
                        Offer_Id = Convert.ToInt32(reader["Offer_Id"]),
                        Offer_Name = reader["Offer_Name"].ToString(),
                        Offer_Percent = Convert.ToDecimal(reader["Offer_Percent"]),
                        Start_Date = Convert.ToDateTime(reader["Start_Date"]),
                        End_Date = Convert.ToDateTime(reader["End_Date"]),
                        Is_Active = Convert.ToBoolean(reader["Is_Active"]),
                        Current_Status = reader["Current_Status"].ToString() // Grabs the dynamic text!
                    });
                }
            }
            return list;
        }

        public Offer GetOfferById(int id)
        {
            Offer offer = null;
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_GetOfferDataById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OfferId", id);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    offer = new Offer
                    {
                        Offer_Id = Convert.ToInt32(reader["Offer_Id"]),
                        Offer_Name = reader["Offer_Name"].ToString(),
                        Offer_Percent = Convert.ToDecimal(reader["Offer_Percent"]),
                        Start_Date = Convert.ToDateTime(reader["Start_Date"]),
                        End_Date = Convert.ToDateTime(reader["End_Date"]),
                        Is_Active = Convert.ToBoolean(reader["Is_Active"])
                    };
                }
            }
            return offer;
        }

        public void AddOffer(Offer offer)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_InsertOfferData", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OfferName", offer.Offer_Name);
                cmd.Parameters.AddWithValue("@OfferPercent", offer.Offer_Percent);
                cmd.Parameters.AddWithValue("@StartDate", offer.Start_Date);
                cmd.Parameters.AddWithValue("@EndDate", offer.End_Date);
                cmd.Parameters.AddWithValue("@IsActive", offer.Is_Active);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateOffer(Offer offer)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateOfferData", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OfferId", offer.Offer_Id);
                cmd.Parameters.AddWithValue("@OfferName", offer.Offer_Name);
                cmd.Parameters.AddWithValue("@OfferPercent", offer.Offer_Percent);
                cmd.Parameters.AddWithValue("@StartDate", offer.Start_Date);
                cmd.Parameters.AddWithValue("@EndDate", offer.End_Date);
                cmd.Parameters.AddWithValue("@IsActive", offer.Is_Active);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteOffer(int id) { ExecuteSimpleSp("sp_DeleteOfferData", id); }
        public void ActivateOffer(int id) { ExecuteSimpleSp("sp_ActivateOfferData", id); }
        public void DeactivateOffer(int id) { ExecuteSimpleSp("sp_DeactivateOfferData", id); }

        private void ExecuteSimpleSp(string spName, int id)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand(spName, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

  
    }
}