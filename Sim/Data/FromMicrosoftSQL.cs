using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Data
{
  public class FromMicrosoftSQL : BaseData
  {
    private static string connectionString = "Data Source=COMPUTADORA;Initial Catalog=BikeStores;Integrated Security=True";
    public static new List<(int, string)> GetAllProducts()
    {
      using (SqlConnection connection = new SqlConnection(connectionString))
      {
        List<(int, string)> output = new List<(int, string)>();
        connection.Open();
        string query = "SELECT p.[product_id], p.[product_name], COUNT(o.[product_id]) " +
          "FROM [BikeStores].[production].[products] AS p JOIN [BikeStores].[sales].[order_items] AS o " +
          "ON p.[product_id] = o.[product_id] " +
          "GROUP BY p.[product_id], o.[product_id], p.[product_name] " +
          "HAVING COUNT(o.[product_id]) > 50 "; //50 should be 200, limits of database sample

        using (SqlCommand command = new SqlCommand(query, connection))
        {
          using (SqlDataReader reader = command.ExecuteReader())
          {
            while (reader.Read())
            {
              output.Add((reader.GetInt32(0), reader.GetString(1)));
            }
            return output;
          }
        }
      }
    }
    public static new List<(DateTime, int)> GetProductSaleData(int productId, int sampleSize)
    {
      using (SqlConnection connection = new SqlConnection(connectionString))
      {
        List<(DateTime, int)> output = new List<(DateTime, int)>();
        connection.Open();
        string query = "SELECT o.[order_date], SUM(oi.[quantity]) " +
          "FROM [BikeStores].[sales].[orders] AS o JOIN [BikeStores].[sales].[order_items] AS oi " +
          "ON o.[order_id] = oi.[order_id] " +
          "WHERE oi.[product_id] = " + productId.ToString() + " " +
          "GROUP BY o.[order_date] ";

        using (SqlCommand command = new SqlCommand(query, connection))
        {
          using (SqlDataReader reader = command.ExecuteReader())
          {
            while (reader.Read())
            {
              output.Add((reader.GetDateTime(0), reader.GetInt32(1)));
            }
            return output;
          }
        }
      }
    }
  }
}
