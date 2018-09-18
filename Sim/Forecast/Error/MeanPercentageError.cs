using System;
using System.Linq;
using Utils.Exceptions;

namespace Forecast.Error
{
  class MeanPercentageError
  {
    public static double Calculation(double[] realValue, double[] forecastValue)
    {
      int n = realValue.Length;
      int m = forecastValue.Length;
      if (0 == n || 0 == m) throw new EmptyParameterArray();
      //Check realValue and forecastValue are the same length
      if (n != m) throw new DifferentSizeArrays();
      //Check if there's any 0 on the realValue array
      if (realValue.Contains(0.0)) throw new ZeroInputArray();
      //Sum of absolute differences divided by real values
      double sumOfDifferences = 
        realValue.Zip(forecastValue, (r, f) => (r - f) / r).Sum();
      return sumOfDifferences / n;
    }
  }
}
