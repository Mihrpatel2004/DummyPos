using DummyPos.Data;
using DummyPos.Models;
using DummyPos.Repositories.Interface;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DummyPos.Repositories.Implementation
{
    public class ItemCatRepo : IItemCatRepo
    {
        private readonly DbHelper _dbHelper;

        public ItemCatRepo(DbHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }
        public Item GetItemById(int id)
        {
            Item item = null;
            using (SqlConnection con = _dbHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("sp_GetItemById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", id);
                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    item = new Item
                    {
                        Item_Id = Convert.ToInt32(reader["Item_Id"]),
                        Item_Name = reader["Item_Name"].ToString(),
                        Amount = Convert.ToDecimal(reader["Amount"]),
                        Item_Category_Id = Convert.ToInt32(reader["Item_Category_Id"]),
                        Prepare_Time_Minutes = reader["Prepare_Time_Minutes"] != DBNull.Value ? Convert.ToInt32(reader["Prepare_Time_Minutes"]) : (int?)null,
                        Is_Active = Convert.ToBoolean(reader["Is_Active"]),
                        Item_Image = reader["Item_Image"] != DBNull.Value ? (byte[])reader["Item_Image"] : null
                    };

                    // BULLETPROOF FIX: We check if the column actually exists in the SQL result before reading it!
                    try
                    {
                        bool hasCategoryColumn = false;
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            if (reader.GetName(i).Equals("Item_Category_Desc", StringComparison.OrdinalIgnoreCase))
                            {
                                hasCategoryColumn = true;
                                break;
                            }
                        }

                        if (hasCategoryColumn && reader["Item_Category_Desc"] != DBNull.Value)
                        {
                            item.Item_Category_Desc = reader["Item_Category_Desc"].ToString();
                        }
                        else
                        {
                            // If the column is missing in SQL, don't crash! Just use a fallback.
                            item.Item_Category_Desc = "Menu Item";
                        }
                    }
                    catch
                    {
                        item.Item_Category_Desc = "Menu Item";
                    }
                }
            }
            return item;
        }
    }
}
