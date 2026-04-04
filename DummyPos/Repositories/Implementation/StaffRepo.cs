using DummyPos.Data;
using DummyPos.Models;
using DummyPos.Repositories.Interface;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DummyPos.Repositories.Implementation
{
    public class StaffRepo : IStaffRepo
    {
        private readonly DbHelper _dbHelper;

        public StaffRepo(DbHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        //---------------------------------------------------------
        // GET ALL STAFF
        //---------------------------------------------------------
        public List<Staffs> GetAllStaff()
        {
            List<Staffs> staffList = new List<Staffs>();

            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("SP_GetAllStaff", con);
                cmd.CommandType = CommandType.StoredProcedure;

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    staffList.Add(new Staffs
                    {
                        Staff_Id = Convert.ToInt32(reader["Staff_Id"]),
                        Staff_Name = reader["Staff_Name"].ToString(),
                        Phone = reader["Phone"].ToString(),
                        Email = reader["Email"].ToString(),
                        Role_Name = reader["Role_Desc"].ToString(),
                        Branch_Name = reader["Branch_Name"].ToString(),
                        Is_Active = Convert.ToBoolean(reader["Is_Active"])
                    });
                }
            }

            return staffList;
        }

        //---------------------------------------------------------
        // GET STAFF BY ID
        //---------------------------------------------------------
        public Staffs GetStaffById(int id)
        {
            Staffs staff = null;

            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("SP_GetStaffById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Staff_Id", id);

                con.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        staff = new Staffs
                        {
                            Staff_Id = Convert.ToInt32(reader["Staff_Id"]),
                            Staff_Name = reader["Staff_Name"].ToString(),
                            Username = reader["Username"].ToString(),
                            Email = reader["Email"].ToString(),
                            Phone = reader["Phone"].ToString(),
                            // ADD THIS ONE LINE:
                            BranchId = reader["BranchId"] != DBNull.Value ? Convert.ToInt32(reader["BranchId"]) : 0,
                            // Mapping the names from the JOINs
                            Role_Name = reader["Role_Desc"].ToString(),
                            Branch_Name = reader["Branch_Name"].ToString(),
                            // Handling null values safely
                            Salary = reader["Salary"] != DBNull.Value ? Convert.ToDecimal(reader["Salary"]) : 0,
                            Hire_Date = reader["Hire_Date"] != DBNull.Value ? Convert.ToDateTime(reader["Hire_Date"]) : (DateTime?)null,
                            Is_Active = Convert.ToBoolean(reader["Is_Active"]),
                            Last_Login = reader["Last_Login"] != DBNull.Value ? Convert.ToDateTime(reader["Last_Login"]) : (DateTime?)null
                        };
                    }
                }
            }
            return staff;
        }
        //---------------------------------------------------------
        // ADD STAFF
        //---------------------------------------------------------
        public void AddStaff(Staffs staff, string plainPassword)
        {
            // Use your PasswordHelper to hash the string
            string hashedPwd = DummyPos.Helpers.PasswordHelper.HashPassword(plainPassword);

            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("SP_AddStaff", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Staff_Name", staff.Staff_Name);
                cmd.Parameters.AddWithValue("@Role_Id", staff.Role_Id);
                cmd.Parameters.AddWithValue("@Phone", staff.Phone ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Email", staff.Email ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Hire_Date", staff.Hire_Date ?? DateTime.Now);
                cmd.Parameters.Add("@Salary", SqlDbType.Decimal).Value = staff.Salary ?? 0;
                cmd.Parameters.AddWithValue("@BranchId", staff.BranchId);
                cmd.Parameters.AddWithValue("@Username", staff.Username);

                // Use the string hash instead of byte array
                cmd.Parameters.AddWithValue("@PasswordHash", hashedPwd);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        //---------------------------------------------------------
        // UPDATE STAFF
        //---------------------------------------------------------
        public void UpdateStaff(Staffs staff)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                // Using your SP_UpdateStaff procedure
                SqlCommand cmd = new SqlCommand("SP_UpdateStaff", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Staff_Id", staff.Staff_Id);
                cmd.Parameters.AddWithValue("@Staff_Name", staff.Staff_Name);
                cmd.Parameters.AddWithValue("@Role_Id", staff.Role_Id);
                cmd.Parameters.AddWithValue("@Phone", staff.Phone ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Email", staff.Email ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Salary", staff.Salary ?? 0);
                cmd.Parameters.AddWithValue("@BranchId", staff.BranchId);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        //---------------------------------------------------------
        // DEACTIVATE STAFF
        //---------------------------------------------------------
        public void DeactivateStaff(int id)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("SP_DeactivateStaff", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Staff_Id", id);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        //---------------------------------------------------------
        // GET ROLES (FOR DROPDOWN)
        //---------------------------------------------------------
        public List<Role> GetRoles()
        {
            List<Role> roles = new List<Role>();

            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Role", con);

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    roles.Add(new Role
                    {
                        Role_Id = Convert.ToInt32(reader["Role_Id"]),
                        Role_Desc = reader["Role_Desc"].ToString()
                    });
                }
            }

            return roles;
        }

        //---------------------------------------------------------
        // GET BRANCH (FOR DROPDOWN)
        //---------------------------------------------------------
        public List<Branch> GetBranch()
        {
            List<Branch> branches = new List<Branch>();

            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM Branch", con);

                con.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    branches.Add(new Branch
                    {
                        Branch_Id = Convert.ToInt32(reader["Branch_Id"]),
                        Branch_Name = reader["Branch_Name"].ToString()
                    });
                }
            }

            return branches;
        }
      
    }
}