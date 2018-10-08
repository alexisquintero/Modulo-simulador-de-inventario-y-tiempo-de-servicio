using System;
using System.Linq;
using Utils.Exceptions;

namespace Forecast.Error
{
  public class RootMeanSquareDeviation
  {
    public static double Calculation(double[] realValue, double[] forecastValue)
    {
      //return the square root of the sum of square diffrences
      return Math.Sqrt(MeanSquaredError.Calculation(realValue, forecastValue));
    }
  }
}
