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
    public static new List<(DateTime, int)> GetProductSaleDataDaily(int productId, int sampleSize)
    {
      using (SqlConnection connection = new SqlConnection(connectionString))
      {
        List<(DateTime, int)> output = new List<(DateTime, int)>();
        connection.Open();
        string query = "SELECT o.[order_date], SUM(oi.[quantity]) " +
          "FROM [BikeStores].[sales].[orders] AS o JOIN [BikeStores].[sales].[order_items] AS oi " +
          "ON o.[order_id] = oi.[order_id] " +
          "WHERE oi.[product_id] = " + productId.ToString() + " " +
          "GROUP BY o.[order_date] " +
          "ORDER BY o.[order_date] ";

        using (SqlCommand command = new SqlCommand(query, connection))
        {
          using (SqlDataReader reader = command.ExecuteReader())
          {
            while (reader.Read())
            {
              output.Add((reader.GetDateTime(0), reader.GetInt32(1)));
            }
            return AddZeroValuePeriodDaily(output);
          }
        }
      }
    }
    public static new List<(DateTime, int)> GetProductSaleDataMonthly(int productId, int sampleSize)
    {
      using (SqlConnection connection = new SqlConnection(connectionString))
      {
        List<(DateTime, int)> output = new List<(DateTime, int)>();
        connection.Open();
        string query = "SELECT DATEPART(YEAR, o.[order_date]) Year, DATEPART(MONTH, o.[order_date]) Month, SUM(oi.[quantity]) " +
          "FROM [BikeStores].[sales].[orders] AS o JOIN [BikeStores].[sales].[order_items] AS oi " +
          "ON o.[order_id] = oi.[order_id] " +
          "WHERE oi.[product_id] = " + productId.ToString() + " " +
          "GROUP BY DATEPART(YEAR, o.[order_date]), DATEPART(MONTH, o.[order_date]) " +
          "ORDER BY DATEPART(YEAR, o.[order_date]), DATEPART(MONTH, o.[order_date]) ";

        using (SqlCommand command = new SqlCommand(query, connection))
        {
          using (SqlDataReader reader = command.ExecuteReader())
          {
            while (reader.Read())
            {
              output.Add(( new DateTime(reader.GetInt32(0), reader.GetInt32(1), 1), reader.GetInt32(2)));
            }
            return AddZeroValuePeriodMonthly(output);
          }
        }
      }
    }
    public static new List<(DateTime, int)> GetProductSaleDataYearly(int productId, int sampleSize)
    {
      using (SqlConnection connection = new SqlConnection(connectionString))
      {
        List<(DateTime, int)> output = new List<(DateTime, int)>();
        connection.Open();
        string query = "SELECT DATEPART(YEAR, o.[order_date]) Year, SUM(oi.[quantity]) " +
          "FROM [BikeStores].[sales].[orders] AS o JOIN [BikeStores].[sales].[order_items] AS oi " +
          "ON o.[order_id] = oi.[order_id] " +
          "WHERE oi.[product_id] = " + productId.ToString() + " " +
          "GROUP BY DATEPART(YEAR, o.[order_date]) " +
          "ORDER BY DATEPART(YEAR, o.[order_date]) ";

        using (SqlCommand command = new SqlCommand(query, connection))
        {
          using (SqlDataReader reader = command.ExecuteReader())
          {
            while (reader.Read())
            {
              output.Add((new DateTime(reader.GetInt32(0), 1, 1), reader.GetInt32(1)));
            }
            return AddZeroValuePeriodYearly(output);
          }
        }
      }
    }
  }
}
