using System;
using System.Linq;

namespace Forecast.Error
{
  class RootMeanSquareDeviation
  {
    public static double Calculation(double[] realValue, double[] forecastValue)
    {
      int n = realValue.Length;
      //Check realValue and forecastValue are the same length
      if (n != forecastValue.Length) throw new Exception("Different input size arrays");
      //Sum of square differences
      double sumOfDifferences = 
        realValue.Zip(forecastValue, (r, f) => Math.Pow(r - f, 2)).Sum();
      //return the square root of the sum of square diffrences
      return Math.Sqrt(sumOfDifferences / n);
    }
  }
}
