using System;
using System.Collections.Generic;
using System.Linq;

namespace Utils.Normalized
{
  public class Zerolized
  {
    public static List<(DateTime, T)> AddZeroValue<T>(List<(DateTime, T)> originalData, Period period)
    {
      switch (period)
      {
        case Period.Diario: return AddZeroValuePeriodDaily<T>(originalData);
        case Period.Mensual: return AddZeroValuePeriodMonthly<T>(originalData);
        case Period.Anual: return AddZeroValuePeriodYearly<T>(originalData);
        default: return new List<(DateTime, T)>();
      }
    }
    private static List<(DateTime, T)> AddZeroValuePeriodDaily<T>(List<(DateTime, T)> originalData)
    {
      List<(DateTime, T)> zerolizedData = new List<(DateTime, T)>();
      //No type safety but no repeating code
      T zero = (T)Convert.ChangeType(0, typeof(T));
      DateTime nextDate = originalData.First().Item1.Date;
      foreach ((DateTime, T) od in originalData)
      {
        while (!od.Item1.Date.Equals(nextDate))
        {
          zerolizedData.Add((nextDate, zero));
          nextDate = nextDate.AddDays(1);
        }
        zerolizedData.Add(od);
        nextDate = nextDate.AddDays(1);
      }
      return zerolizedData;
    }
    private static List<(DateTime, T)> AddZeroValuePeriodMonthly<T>(List<(DateTime, T)> originalData)
    {
      List<(DateTime, T)> zerolizedData = new List<(DateTime, T)>();
      T zero = (T)Convert.ChangeType(0, typeof(T));
      DateTime nextDate = originalData.First().Item1.Date;
      foreach ((DateTime, T) od in originalData)
      {
        while (!od.Item1.Date.Equals(nextDate))
        {
          zerolizedData.Add((nextDate, zero));
          nextDate = nextDate.AddMonths(1);
        }
        zerolizedData.Add(od);
        nextDate = nextDate.AddMonths(1);
      }
      return zerolizedData;
    }
    private static List<(DateTime, T)> AddZeroValuePeriodYearly<T>(List<(DateTime, T)> originalData)
    {
      List<(DateTime, T)> zerolizedData = new List<(DateTime, T)>();
      T zero = (T)Convert.ChangeType(0, typeof(T));
      DateTime nextDate = originalData.First().Item1.Date;
      foreach ((DateTime, T) od in originalData)
      {
        while (!od.Item1.Date.Equals(nextDate))
        {
          zerolizedData.Add((nextDate, zero));
          nextDate = nextDate.AddYears(1);
        }
        zerolizedData.Add(od);
        nextDate = nextDate.AddYears(1);
      }
      return zerolizedData;
    }
  }
}
