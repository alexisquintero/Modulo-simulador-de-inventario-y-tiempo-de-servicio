using MathNet.Numerics;
using System;
using System.Linq;

namespace Forecast.Method.LinearRegression
{
  //Least squared
  class SimpleLinearRegression
  {
    public static double[] Calculate(double[] inputValue, int amountOfPeriodsToCalculate)
    {
      int inputLength = inputValue.Length + 1;
      double[] xData = Enumerable.Range(0, inputLength).Select(i => (double)i).ToArray();
      Tuple<double, double> parameters = Fit.Line(xData, inputValue);
      return Enumerable.Range(0, inputLength + amountOfPeriodsToCalculate).Select(i => parameters.Item1 + parameters.Item2 * i).ToArray();
    }      
  }
}