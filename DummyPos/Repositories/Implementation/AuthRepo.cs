using DummyPos.Data;
using DummyPos.Models;
using DummyPos.Repositories.Interface;
using Microsoft.Data.SqlClient;
using System;
using System.Data;

namespace DummyPos.Repositories.Implementation
{
    public class AuthRepo : IAuthRepo
    {
        private readonly DbHelper _dbHelper;

        public AuthRepo(DbHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public LoggedInUser ValidateLogin(string email, string plainPassword)
        {
            LoggedInUser user = null;
            string hashedPwdToVerify = DummyPos.Helpers.PasswordHelper.HashPassword(plainPassword);

            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_StaffLogin", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Email", email);

                cmd.CommandTimeout = 120;
                con.Open();

                // 1. Read the data inside a 'using' block
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string dbPasswordHash = reader["PasswordHash"].ToString();

                        if (dbPasswordHash == hashedPwdToVerify)
                        {
                            user = new LoggedInUser
                            {
                                Staff_Id = Convert.ToInt32(reader["Staff_Id"]),
                                Staff_Name = reader["Staff_Name"].ToString(),
                                Email = reader["Email"].ToString(),
                                RoleName = reader["RoleName"].ToString()
                            };
                        }
                    }
                } // <--- MAGIC HAPPENS HERE: The reader automatically closes when it hits this bracket!

                // 2. NOW it is 100% safe to run the update command on the connection
                if (user != null)
                {
                    SqlCommand updateCmd = new SqlCommand("sp_UpdateLastLogin", con);
                    updateCmd.CommandType = CommandType.StoredProcedure;
                    updateCmd.Parameters.AddWithValue("@Staff_Id", user.Staff_Id);
                    updateCmd.ExecuteNonQuery();
                }
            }
            return user;
        }

        //

        public bool ChangePassword(string email, string oldPassword, string newPassword)
        {
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("sp_ChangePasswordByEmail", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@OldPassword", oldPassword);
                    cmd.Parameters.AddWithValue("@NewPassword", newPassword);

                    con.Open();
                    // Returns true if the email and old password matched and the update succeeded
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
        }
    }
}