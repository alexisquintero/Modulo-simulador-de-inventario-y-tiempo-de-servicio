using System;
using System.Linq;
using Utils;
using Utils.Exceptions;

namespace Forecast.Error
{
  public class MeanPercentageError
  {
    public static double Calculation(double[] realValue, double[] forecastValue)
    {
      double[] newReal = ArrayBased.ShortenRealValueArray(realValue, forecastValue);
      int n = newReal.Length;
      int m = forecastValue.Length;
      if (0 == n || 0 == m) throw new EmptyParameterArray();
      //Check if there's any 0 on the realValue array
      if (newReal.Contains(0.0)) return -1;  //throw new ZeroInputArray();
      //Sum of absolute differences divided by real values
      double sumOfDifferences = newReal.Zip(forecastValue, (r, f) => (r - f) / r).Sum();
      return sumOfDifferences / n;
    }
  }
}
