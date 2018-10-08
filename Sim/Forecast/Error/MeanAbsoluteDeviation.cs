using System;
using System.Linq;
using Utils.Exceptions;

namespace Forecast.Error
{
  public class MeanAbsoluteDeviation
  {
    public static double Calculation(double[] realValue, double[] forecastValue)
    {
      int n = realValue.Length;
      int m = forecastValue.Length;
      if (0 == n || 0 == m) throw new EmptyParameterArray();
      //Check realValue and forecastValue are the same length
      if (n != m) throw new DifferentSizeArrays();
      //Sum of absolute differences
      double sumOfDifferences = 
        realValue.Zip(forecastValue, (r, f) => Math.Abs(r - f)).Sum();
      return sumOfDifferences / n;
    }
  }
}
