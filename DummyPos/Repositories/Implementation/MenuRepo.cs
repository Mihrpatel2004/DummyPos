using DummyPos.Data;
using DummyPos.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace DummyPos.Repositories.Implementation
{
    public class MenuRepo : IMenuRepo
    {
        private readonly DbHelper _dbHelper;

        public MenuRepo(DbHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        public List<ItemSearchViewModel> SearchItems(string keyword)
        {
            List<ItemSearchViewModel> results = new List<ItemSearchViewModel>();

            using (SqlConnection con = _dbHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("sp_SearchMenuItems", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Sending the text from the search bar to your SP
                    cmd.Parameters.AddWithValue("@SearchQuery", keyword);

                    con.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // 1. Create a default placeholder image just in case the DB image is missing
                            string imageUrl = "https://via.placeholder.com/300x200?text=No+Image";

                            // 2. Check if the database actually has an image saved for this item
                            if (reader["Item_Image"] != DBNull.Value)
                            {
                                // Convert the raw SQL binary data into a byte array
                                byte[] imageBytes = (byte[])reader["Item_Image"];

                                // Convert the byte array into a Base64 string
                                string base64String = Convert.ToBase64String(imageBytes);

                                // Format it perfectly for the HTML <img> tag
                                imageUrl = $"data:image/png;base64,{base64String}";
                            }

                            // 3. Add the item to your list
                            results.Add(new ItemSearchViewModel
                            {
                                Item_Id = Convert.ToInt32(reader["Item_Id"]),
                                Item_Name = reader["Item_Name"].ToString(),
                                Amount = Convert.ToDecimal(reader["Amount"]),
                                ImageDataUrl = imageUrl // 🚨 Pass the converted image here!
                            });
                        }
                    }
                }
            }
            return results;
        }
    }
}