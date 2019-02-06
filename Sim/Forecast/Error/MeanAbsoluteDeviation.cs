using System;
using System.Linq;
using Utils;
using Utils.Exceptions;

namespace Forecast.Error
{
  public class MeanAbsoluteDeviation
  {
    public static double Calculation(double[] realValue, double[] forecastValue)
    {
      double[] newReal = ArrayBased.ShortenRealValueArray(realValue, forecastValue);
      double n = newReal.Length;
      int m = forecastValue.Length;
      if (0 == n || 0 == m) return -1;// throw new EmptyParameterArray();
      //Sum of absolute differences
      double sumOfDifferences = 
        newReal.Zip(forecastValue, (r, f) => Math.Abs(r - f)).Sum();
      return sumOfDifferences / n;
    }
  }
}
