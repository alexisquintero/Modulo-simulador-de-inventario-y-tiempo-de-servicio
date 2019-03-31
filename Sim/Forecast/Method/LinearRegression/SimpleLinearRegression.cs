using MathNet.Numerics;
using System;
using System.Linq;
using Utils;

namespace Forecast.Method.LinearRegression
{
  //Least squared
  public class SimpleLinearRegression
  {
    public static string Name = "Simple Linear Regression";
    public static (double[], double[]) Calculate(double[] inputValue, int amountOfPeriodsToCalculate)
    {
      Name = "Simple Linear Regression";
      int inputLength = inputValue.Length;
      double[] xData = Enumerable.Range(0, inputLength).Select(i => (double)i).ToArray();
      Tuple<double, double> parameters = Fit.Line(xData, inputValue);
      double[] full = Enumerable.Range(0, inputLength + amountOfPeriodsToCalculate)
        .Select(i => parameters.Item1 + parameters.Item2 * i).ToArray();
      //Check if a value is less than 0
      if (full.Where(f => f < 0).Count() > 0) Name = "!!!" + Name;
      return ArrayBased.Split(full, amountOfPeriodsToCalculate);
    }      
  }
}