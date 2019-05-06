using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Data
{
  public class FromMicrosoftSQL : BaseData
  {
    private static string connectionString = "Data Source=COMPUTADORA;Initial Catalog=DatosReales;Integrated Security=True";
    public static new List<(int, string)> GetAllProducts()
    {
      using (SqlConnection connection = new SqlConnection(connectionString))
      {
        List<(int, string)> output = new List<(int, string)>();
        connection.Open();
        string query = "SELECT DISTINCT [codigo], [descripcion] " +
          "FROM [DatosReales].[dbo].[base_stock]";

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
        string query = "SELECT TOP(500) [FechaDate], SUM([Cantidad]) " +
          "FROM [DatosReales].[dbo].[base_stock] " +
          "WHERE [codigo] = " + productId.ToString() + " " +
          "GROUP BY [FechaDate] " +
          "ORDER BY [FechaDate];";

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
    public static new List<(DateTime, int)> GetProductSaleDataMonthly(int productId, int sampleSize)
    {
      using (SqlConnection connection = new SqlConnection(connectionString))
      {
        List<(DateTime, int)> output = new List<(DateTime, int)>();
        connection.Open();
        string query = "SELECT DATEPART(YEAR, [FechaDate]) Year, DATEPART(MONTH, [FechaDate]) Month, SUM([Cantidad]) " +
          "FROM [DatosReales].[dbo].[base_stock] " +
          "WHERE [codigo] = " + productId.ToString() + " " +
          "GROUP BY DATEPART(YEAR, [FechaDate]), DATEPART(MONTH, [FechaDate]) " +
          "ORDER BY DATEPART(YEAR, [FechaDate]), DATEPART(MONTH, [FechaDate])";

        using (SqlCommand command = new SqlCommand(query, connection))
        {
          using (SqlDataReader reader = command.ExecuteReader())
          {
            while (reader.Read())
            {
              output.Add(( new DateTime(reader.GetInt32(0), reader.GetInt32(1), 1), reader.GetInt32(2)));
            }
            return output;
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
        string query = "SELECT DATEPART(YEAR, [FechaDate]) Year, SUM([Cantidad]) " +
          "FROM [DatosReales].[dbo].[base_stock] " +
          "WHERE [codigo] = " + productId.ToString() + " " +
          "GROUP BY DATEPART(YEAR, [FechaDate])" +
          "ORDER BY DATEPART(YEAR, [FechaDate])";

        using (SqlCommand command = new SqlCommand(query, connection))
        {
          using (SqlDataReader reader = command.ExecuteReader())
          {
            while (reader.Read())
            {
              output.Add((new DateTime(reader.GetInt32(0), 1, 1), reader.GetInt32(1)));
            }
            return output;
          }
        }
      }
    }
  }
}
