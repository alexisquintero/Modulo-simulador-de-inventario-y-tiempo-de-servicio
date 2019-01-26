using System;
using System.Collections.Generic;
using System.Linq;
using Utils.Exceptions;

namespace Data
{
  public abstract class BaseData
  {
    public static List<(int, string)> GetAllProducts() { throw new BaseDataMethod(); }
    public static List<(DateTime, int)> GetProductSaleDataDaily(int productId, int sampleSize) { throw new BaseDataMethod(); }
    public static List<(DateTime, int)> GetProductSaleDataMonthly(int productId, int sampleSize) { throw new BaseDataMethod(); }
    public static List<(DateTime, int)> GetProductSaleDataYearly(int productId, int sampleSize) { throw new BaseDataMethod(); }
    protected static List<(DateTime, int)> AddZeroValuePeriodDaily(List<(DateTime, int)> originalData)
    {
      List<(DateTime, int)> zerolizedData = new List<(DateTime, int)>();
      DateTime nextDate = originalData.First().Item1.Date;
      foreach ((DateTime, int) od in originalData)
      {
        while (!od.Item1.Date.Equals(nextDate))
        {
          zerolizedData.Add((nextDate, 0));
          nextDate = nextDate.AddDays(1);
        }
        zerolizedData.Add(od);
        nextDate = nextDate.AddDays(1);
      }
      return zerolizedData;
    }
    //protected static List<(DateTime, int)> AddZeroValuePeriodMonthly(List<(DateTime, int)> originalData)
    //protected static List<(DateTime, int)> AddZeroValuePeriodYearly(List<(DateTime, int)> originalData)
  }
}
