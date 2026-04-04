using DummyPos.Data;
using DummyPos.Models;
using DummyPos.Repositories.Interface;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DummyPos.Repositories.Implementation
{
    public class RoleRepo : IRoleRepo
    {
        private readonly DbHelper _dbHelper;

        public RoleRepo(DbHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }
        public void ActivateRole(int id)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_ActivateRole", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Role_Id", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public void DeactivateRole(int id)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_DeactivateRole", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Role_Id", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void AddRole(Role role)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_InsertRole", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Role_Desc", role.Role_Desc);
                cmd.Parameters.AddWithValue("@Is_Active", role.Is_Active);
                // Hardcoded for now, or pull from User.Identity.Name
                cmd.Parameters.AddWithValue("@Created_By", "Admin");

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public List<Role> GetAllRole()
        {
            List<Role> roles = new List<Role>();

            using (SqlConnection con = _dbHelper.GetConnection())
            {
                // 1. Setup the command
                SqlCommand cmd = new SqlCommand("sp_GetAllRole", con);
                cmd.CommandType = CommandType.StoredProcedure;

                // 2. Open connection and execute
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                // 3. Map the database rows to your C# Role objects
                while (reader.Read())
                {
                    roles.Add(new Role
                    {
                        Role_Id = Convert.ToInt32(reader["Role_Id"]),
                        Role_Desc = reader["Role_Desc"].ToString(),
                        Is_Active = Convert.ToBoolean(reader["Is_Active"]),
                        Created_Date = reader["Created_Date"] != DBNull.Value ? Convert.ToDateTime(reader["Created_Date"]) : (DateTime?)null,
                        Created_By = reader["Created_By"].ToString(),
                        Modified_Date = reader["Modified_Date"] != DBNull.Value ? Convert.ToDateTime(reader["Modified_Date"]) : (DateTime?)null,
                        Modified_By = reader["Modified_By"].ToString()
                    });
                }
            }

            // 4. Return the populated list
            return roles;
        }

        public Role GetRoleById(int id)
        {
            Role role = null;

            using (SqlConnection con = _dbHelper.GetConnection())
            {
                // 1. Setup the command with the SP name
                SqlCommand cmd = new SqlCommand("sp_GetRoleById", con);
                cmd.CommandType = CommandType.StoredProcedure;

                // 2. Add the parameter required by the SP
                cmd.Parameters.AddWithValue("@Role_Id", id);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                // 3. Map the single row result to the Role object
                if (reader.Read())
                {
                    role = new Role
                    {
                        Role_Id = Convert.ToInt32(reader["Role_Id"]),
                        Role_Desc = reader["Role_Desc"]?.ToString() ?? string.Empty,
                        Is_Active = Convert.ToBoolean(reader["Is_Active"]),

                        // Handling nullable DateTime fields safely
                        Created_Date = reader["Created_Date"] != DBNull.Value ? Convert.ToDateTime(reader["Created_Date"]) : null,
                        Created_By = reader["Created_By"]?.ToString() ?? string.Empty,

                        Modified_Date = reader["Modified_Date"] != DBNull.Value ? Convert.ToDateTime(reader["Modified_Date"]) : null,
                        Modified_By = reader["Modified_By"]?.ToString() ?? string.Empty
                    };
                }
            }

            return role;
        }

        public void UpdateRole(Role role)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateRole", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Role_Id", role.Role_Id);
                cmd.Parameters.AddWithValue("@Role_Desc", role.Role_Desc);
                cmd.Parameters.AddWithValue("@Is_Active", role.Is_Active);
                // Assuming 'Admin' for now; in a real app, use the logged-in user's name
                cmd.Parameters.AddWithValue("@Modified_By", "Admin");

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public void DeleteRole(int id)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_DeleteRole", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Role_Id", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}
