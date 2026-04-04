using DummyPos.Data;
using DummyPos.Models;
using DummyPos.Repositories.Interface;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace DummyPos.Repositories.Implementation
{
    public class KitchenStationRepo : IKitchenStationRepo
    {
        private readonly DbHelper _dbHelper;
        public KitchenStationRepo(DbHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }
       

        public List<KitchenStation> GetStationsByBranchId(int branchId)
        {
            var list = new List<KitchenStation>();
            using (var con = _dbHelper.GetConnection())
            {
                var cmd = new SqlCommand("sp_KS_GetByBranchId", con) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@BranchId", branchId);
                con.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new KitchenStation
                    {
                        Station_Id = Convert.ToInt32(reader["Station_Id"]),
                        Station_Name = reader["Station_Name"].ToString(),
                        Branch_Id = Convert.ToInt32(reader["Branch_Id"]),
                        Is_Active = Convert.ToBoolean(reader["Is_Active"]),
                        Branch_Name = reader["Branch_Name"].ToString()
                    });
                }
            }
            return list;
        }

        public KitchenStation GetStationById(int id)
        {
            KitchenStation station = null;
            using (var con = _dbHelper.GetConnection())
            {
                var cmd = new SqlCommand("sp_KS_GetById", con) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                var reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    station = new KitchenStation
                    {
                        Station_Id = Convert.ToInt32(reader["Station_Id"]),
                        Station_Name = reader["Station_Name"].ToString(),
                        Branch_Id = Convert.ToInt32(reader["Branch_Id"]),
                        Is_Active = Convert.ToBoolean(reader["Is_Active"])
                    };
                }
            }
            return station;
        }

        public void AddStation(KitchenStation station)
        {
            using (var con = _dbHelper.GetConnection())
            {
                var cmd = new SqlCommand("sp_KS_Insert", con) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@StationName", station.Station_Name);
                cmd.Parameters.AddWithValue("@BranchId", station.Branch_Id);
                cmd.Parameters.AddWithValue("@IsActive", station.Is_Active);
                con.Open(); cmd.ExecuteNonQuery();
            }
        }

        public void UpdateStation(KitchenStation station)
        {
            using (var con = _dbHelper.GetConnection())
            {
                var cmd = new SqlCommand("sp_KS_Update", con) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@Id", station.Station_Id);
                cmd.Parameters.AddWithValue("@StationName", station.Station_Name);
                cmd.Parameters.AddWithValue("@IsActive", station.Is_Active);
                con.Open(); cmd.ExecuteNonQuery();
            }
        }

        public void DeleteStation(int id)
        {
            using (var con = _dbHelper.GetConnection())
            {
                var cmd = new SqlCommand("sp_KS_Delete", con) { CommandType = CommandType.StoredProcedure };
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open(); cmd.ExecuteNonQuery();
            }
        }

        /*  public void ToggleActive(int id, bool status)
          {
              using (var con = _dbHelper.GetConnection())
              {
                  var cmd = new SqlCommand("sp_KS_ToggleActive", con) { CommandType = CommandType.StoredProcedure };
                  cmd.Parameters.AddWithValue("@Id", id);
                  cmd.Parameters.AddWithValue("@Status", status);
                  con.Open(); cmd.ExecuteNonQuery();
              }
          }
  */
        public void ActivateK_S(int id)
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
        }
    }
}