using System.Collections.Generic;
using System.Linq;

namespace Forecast.Method.AverageBased
{
  public class HoltHelper
  {
    public static List<double> Calculate(List<double> smoothedValues, List<double> trendValues, List<double> output)
    {
      if(0 == smoothedValues.Count) { return output; }
      else {
        double next = Forecast(smoothedValues.First(), trendValues.First(), 1);
        output.Add(next);
      }
      return Calculate(smoothedValues.Skip(1).ToList(), trendValues.Skip(1).ToList(), output);
    }
    public static decimal SmoothedValue(decimal dataSmoothingFactor, decimal realValue,
      decimal previousSmoothedValue, decimal previousTrendValue)
    {
      return dataSmoothingFactor * realValue + (1 - dataSmoothingFactor) * (previousSmoothedValue + previousTrendValue);
    } 
    public static decimal TrendValue(decimal trendSmoothingFactor, decimal smoothedValue,
      decimal previousSmoothedValue, decimal previousTrendValue)
    {
      return trendSmoothingFactor * (smoothedValue - previousSmoothedValue) + (1 - trendSmoothingFactor) * previousTrendValue;
    }
    public static double Forecast(double smoothedValue, double trendValue, int index)
    {
      return smoothedValue + index * trendValue;
    }
    //Starting trend is 1
    public static (List<double>, List<double>) CalculteSmoothedAndTrendValues(List<decimal> realValuesDouble,
      decimal dataSmoothingFactor, decimal trendSmoothingFactor, List<decimal> smoothedValues, List<decimal> trendValues,
      bool firstRunFlag = true)
    {
      List<decimal> realValues = realValuesDouble.Select(d => (decimal)d).ToList();
      if (0 == realValues.Count)
      { return (smoothedValues.Select(m => (double)m).ToList(), trendValues.Select(m => (double)m).ToList()); }
      //First trend value is always zero
      //First smoothed value is always the same as the real value
      if (firstRunFlag)
      {
        smoothedValues.Add(SmoothedValue(dataSmoothingFactor, realValues.First(), realValues.First(), 1));
        trendValues.Add(0.0m);
      }
      else
      {
        smoothedValues.Add(
          SmoothedValue(dataSmoothingFactor, realValues.First(), smoothedValues.Last(), trendValues.Last()));
        trendValues.Add(TrendValue(
          trendSmoothingFactor, smoothedValues.Last(), smoothedValues.ElementAt(smoothedValues.Count - 2),
          trendValues.Last()));
      }
      return CalculteSmoothedAndTrendValues(
        realValues.Skip(1).ToList(), dataSmoothingFactor, trendSmoothingFactor, smoothedValues, trendValues, false);
    }
  }
}
