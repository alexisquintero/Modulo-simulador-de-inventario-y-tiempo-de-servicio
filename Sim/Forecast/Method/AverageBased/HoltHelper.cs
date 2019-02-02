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
    public static double SmoothedValue(double dataSmoothingFactor, double realValue,
      double previousSmoothedValue, double previousTrendValue)
    {
      return dataSmoothingFactor * realValue + (1 - dataSmoothingFactor) * (previousSmoothedValue + previousTrendValue);
    } 
    public static double TrendValue(double trendSmoothingFactor, double smoothedValue,
      double previousSmoothedValue, double previousTrendValue)
    {
      return trendSmoothingFactor * (smoothedValue - previousSmoothedValue) + (1 - trendSmoothingFactor) * previousTrendValue;
    }
    public static double Forecast(double smoothedValue, double trendValue, int index)
    {
      return smoothedValue + index * trendValue;
    }
    //Starting trend is 1
    public static (List<double>, List<double>) CalculteSmoothedAndTrendValues(List<double> realValues,
      double dataSmoothingFactor, double trendSmoothingFactor, List<double> smoothedValues, List<double> trendValues,
      bool firstRunFlag = true)
    {
      if (0 == realValues.Count) return (smoothedValues, trendValues);
      //First trend value is always zero
      //First smoothed value is always the same as the real value
      if (firstRunFlag)
      {
        smoothedValues.Add( SmoothedValue( dataSmoothingFactor, realValues.First(), realValues.First(), 1));
        trendValues.Add(0.0);
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
