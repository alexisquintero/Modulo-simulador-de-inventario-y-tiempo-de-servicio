using System;
using System.Linq;
using Utils;
using Utils.Exceptions;

namespace Forecast.Error
{
  public class MeanSquaredError
  {
    public static double Calculation(double[] realValue, double[] forecastValue)
    {
      double[] newReal = ArrayBased.ShortenRealValueArray(realValue, forecastValue);
      int n = newReal.Length;
      int m = forecastValue.Length;
      if (0 == n || 0 == m) return -1;// throw new EmptyParameterArray();
      //Sum of square differences
      double sumOfDifferences = 
        newReal.Zip(forecastValue, (r, f) => Math.Pow(r - f, 2)).Sum();
      return sumOfDifferences / n;
    }
  }
}
