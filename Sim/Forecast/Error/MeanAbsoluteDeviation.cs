using System;
using System.Linq;

namespace Forecast.Error
{
  class MeanAbsoluteDeviation
  {
    public static double Calculation(double[] realValue, double[] forecastValue)
    {
      int n = realValue.Length;
      //Check realValue and forecastValue are the same length
      if (n != forecastValue.Length) throw new Exception("Different input size arrays");
      //Sum of absolute differences
      double sumOfDifferences = 
        realValue.Zip(forecastValue, (r, f) => Math.Abs(r - f)).Sum();
      return sumOfDifferences / n;
    }
  }
}
