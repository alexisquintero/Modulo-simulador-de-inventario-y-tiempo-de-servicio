using System;
using System.Linq;

namespace Forecast.Error
{
  class MeanAbsolutePercentageError
  {
    public static double Calculation(double[] realValue, double[] forecastValue)
    {
      int n = realValue.Length;
      //Check realValue and forecastValue are the same length
      if (n != forecastValue.Length) throw new Exception("Different input size arrays");
      //Check if there's any 0 on the realValue array
      if (realValue.Contains(0.0)) throw new Exception("There's a zero on the real array");
      //Sum of absolute differences
      double sumOfDifferences = 
        realValue.Zip(forecastValue, (r, f) => Math.Abs(r - f) / Math.Abs(r)).Sum();
      return sumOfDifferences / n;
    }
  }
}
