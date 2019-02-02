using Forecast.Error;
using System.Collections.Generic;
using System.Linq;
using Utils;

namespace Forecast.Method.AverageBased
{
  public class Holt
  {
    public static string Name = "Holt";
    public static (double[], double[]) Calculate(
      double[] inputValue, int amountOfPeriodsToCalculate, double dataSmoothingFactor, double trendSmoothingFactor) 
    {
      double initalSmoothedValue = inputValue[0];
      (List<double>, List<double>) auxValues = HoltHelper.CalculteSmoothedAndTrendValues(
        inputValue.ToList(), dataSmoothingFactor, trendSmoothingFactor, new List<double>(), new List<double>());
      //Calculate forecast for periods which already have real values
      double[] full =
        HoltHelper.Calculate(auxValues.Item1, auxValues.Item2, inputValue.Take(1).ToList()).ToArray<double>();
      return ArrayBased.Split(full, amountOfPeriodsToCalculate);
    }
    public static (double[], double[]) CalculateBest(double[] inputValue, int amountOfPeriodsToCalculate)
    {
      List<(double[], double[])> all = new List<(double[], double[])>();
      for (double d = 0.1; d < 1.0; d+=0.1)
      {
        for (double t = 0.1; t < 1.0; t+=0.1)
        {
          all.Add(Calculate(inputValue, amountOfPeriodsToCalculate, d, t));
        }
      }

      List<(double, (double[], double[]))> allWithError = new List<(double, (double[], double[]))>();
      foreach ((double[], double[]) f in all)
      {
        double mad = MeanAbsoluteDeviation.Calculation(inputValue, f.Item1);
        allWithError.Add((mad, f));
      }
      List<(double, (double[], double[]))> allWithErrorSorted = allWithError.OrderBy(a => a.Item1).ToList();

      return allWithErrorSorted.First().Item2;
    }
  }
}
