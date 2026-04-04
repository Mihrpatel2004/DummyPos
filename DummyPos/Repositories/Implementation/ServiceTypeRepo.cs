using DummyPos.Data;
using DummyPos.Models;
using DummyPos.Repositories.Interface;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DummyPos.Repositories.Implementation
{
    public class ServiceTypeRepo : IServiceTypeRepo
    {
        private readonly DbHelper _dbHelper;

        public ServiceTypeRepo(DbHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

      

        public void AddServiceType(ServiceType service)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_InsertServiceType", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Desc", service.Service_Type_Desc);
                cmd.Parameters.AddWithValue("@IsActive", service.Is_Active);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

       

        public List<ServiceType> GetAllServiceTypes()
        {
            List<ServiceType> list = new List<ServiceType>();
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_GetAllServiceTypes", con);
                cmd.CommandType = CommandType.StoredProcedure;
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    list.Add(new ServiceType
                    {
                        Service_Type_Id = Convert.ToInt32(reader["Service_Type_Id"]),
                        Service_Type_Desc = reader["Service_Type_Desc"].ToString(),
                        Is_Active = Convert.ToBoolean(reader["Is_Active"]),
                        Created_Date = reader["Created_Date"] != DBNull.Value ? Convert.ToDateTime(reader["Created_Date"]) : (DateTime?)null
                    });
                }
            }
            return list;
        }

        public ServiceType GetServiceTypeById(int id)
        {
            ServiceType service = null;
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_GetServiceTypeById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    service = new ServiceType
                    {
                        Service_Type_Id = Convert.ToInt32(reader["Service_Type_Id"]),
                        Service_Type_Desc = reader["Service_Type_Desc"].ToString(),
                        Is_Active = Convert.ToBoolean(reader["Is_Active"]),
                        Created_Date = reader["Created_Date"] != DBNull.Value ? Convert.ToDateTime(reader["Created_Date"]) : (DateTime?)null
                    };
                }
            }
            return service;
        }
        public void UpdateServiceType(ServiceType service)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateServiceType", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", service.Service_Type_Id);
                cmd.Parameters.AddWithValue("@Desc", service.Service_Type_Desc);
                cmd.Parameters.AddWithValue("@IsActive", service.Is_Active);
                con.Open();
               cmd.ExecuteNonQuery();
            }
        }

        public void ActivateServiceType(int id)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_ActivateServiceType", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeactivateServiceType(int id)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_DeactivateServiceType", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
