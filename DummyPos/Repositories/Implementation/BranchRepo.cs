using DummyPos.Data;
using DummyPos.Models;
using DummyPos.Repositories.Interface;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DummyPos.Repositories.Implementation
{
    public class BranchRepo : IBranchRepo
    {
        private readonly DbHelper _dbHelper;

        public BranchRepo(DbHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public void AddBranch(Branch branch)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_InsertBranch", con);
                cmd.CommandType = CommandType.StoredProcedure;

                // Pass parameters to the SP
                cmd.Parameters.AddWithValue("@Branch_Name", branch.Branch_Name);
                cmd.Parameters.AddWithValue("@Is_Active", branch.Is_Active);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public List<Branch> GetAllBranchList()
        {
            List<Branch> branch = new List<Branch>();

            using (SqlConnection con = _dbHelper.GetConnection())
            {
                // 1. Setup the command
                SqlCommand cmd = new SqlCommand("sp_GetAllBranch", con);
                cmd.CommandType = CommandType.StoredProcedure;

                // 2. Open connection and execute
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                // 3. Map the database rows to your C# Role objects
                while (reader.Read())
                {
                    branch.Add(new Branch
                    {
                        Branch_Id = Convert.ToInt32(reader["Branch_Id"]),
                        Branch_Name = reader["Branch_Name"].ToString(),
                        Is_Active = Convert.ToBoolean(reader["Is_Active"]),
                        Created_Date = reader["Created_Date"] != DBNull.Value ? Convert.ToDateTime(reader["Created_Date"]) : (DateTime?)null

                    });
                }
            }
            return branch;
        }

        public Branch GetBranchById(int id)
        {
            Branch branch = null;

            using (SqlConnection con = _dbHelper.GetConnection())
            {
                // 1. Setup the command to use the Stored Procedure
                SqlCommand cmd = new SqlCommand("sp_GetBranchById", con);
                cmd.CommandType = CommandType.StoredProcedure;

                // 2. Add the parameter
                cmd.Parameters.AddWithValue("@Branch_Id", id);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                // 3. Map the single row to the Branch object
                if (reader.Read())
                {
                    branch = new Branch
                    {
                        Branch_Id = Convert.ToInt32(reader["Branch_Id"]),
                        Branch_Name = reader["Branch_Name"].ToString(),
                        Is_Active = Convert.ToBoolean(reader["Is_Active"]),
                        // Handling nullable Created_Date as you did in GetAll
                        Created_Date = reader["Created_Date"] != DBNull.Value
                                       ? Convert.ToDateTime(reader["Created_Date"])
                                       : (DateTime?)null
                    };
                }
            }
            return branch;
        }
        public void DeactivateBranch(int id)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_DeactivateBranch", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Branch_Id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void ActivateBranch(int id)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_ActivateBranch", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Branch_Id", id);
                con.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public Branch GetBranchDetails(int id)
        {
            Branch branch = null;

            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_GetBranchDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Branch_Id", id);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    branch = new Branch
                    {
                        Branch_Id = Convert.ToInt32(reader["Branch_Id"]),
                        Branch_Name = reader["Branch_Name"].ToString(),
                        Is_Active = Convert.ToBoolean(reader["Is_Active"]),
                        Created_Date = reader["Created_Date"] != DBNull.Value
                                       ? Convert.ToDateTime(reader["Created_Date"])
                                       : (DateTime?)null
                    };
                }
            }
            return branch;
        }
        public void UpdateBranch(Branch branch)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_UpdateBranch", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Branch_Id", branch.Branch_Id);
                cmd.Parameters.AddWithValue("@Branch_Name", branch.Branch_Name);
                cmd.Parameters.AddWithValue("@Is_Active", branch.Is_Active);

                con.Open();
                cmd.ExecuteNonQuery();
               
            }
        }

       
    }
}