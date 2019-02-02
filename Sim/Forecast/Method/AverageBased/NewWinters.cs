using Forecast.Error;
using System.Collections.Generic;
using System.Linq;

namespace Forecast.Method.AverageBased
{
  public class NewWinters
  {
    public static string Name = "New Winters";
    public static (double[], double[]) Calculate(double[] Y, double alpha, double beta, double gamma, int p, int s)
    {
      NewWintersHelper.Init(Y, s, alpha, beta, gamma);
      return NewWintersHelper.Calculate(p, s);
    }
    public static (double[], double[]) CalculateBest(double[] inputValue, int amountOfPeriodsToCalculate, int s)
    {
      List<(double[], double[])> all = new List<(double[], double[])>();
      for (double a = 0.1; a < 1.0; a+=0.1)
      {
        for (double b = 0.1; b < 1.0; b+=0.1)
        {
          for (double g = 0.1; g < 1.0; g+=0.1)
          {
            all.Add(Calculate(inputValue, a, b, g,amountOfPeriodsToCalculate, s));
          }
        }
      }

      List<(double, (double[], double[]))> allWithError = new List<(double, (double[], double[]))>();
      foreach ((double[], double[]) f in all)
      {
        double mad = MeanAbsoluteDeviation.Calculation(inputValue, f.Item1);
        allWithError.Add((mad, f));
      }
      allWithError.RemoveAll(a => double.IsNaN(a.Item1));
      List<(double, (double[], double[]))> allWithErrorSorted = allWithError.OrderBy(a => a.Item1).ToList();

      return allWithErrorSorted.First().Item2;
    }
  }
}
