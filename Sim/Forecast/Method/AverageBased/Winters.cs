using Forecast.Error;
using System.Collections.Generic;
using System.Linq;
using Utils;

namespace Forecast.Method.AverageBased
{
  public class Winters 
  {
    public static string Name = "Winters";
    public static (double[], double[]) Calculate(double[] inputValues, double levelConstant, double trendConstant,
      double seasonalConstant, int amountOfPeriodsToCalculate, int seasonLength = 4)
    {
      (List<double>, List<double>, List<double>) initialCalculations =
        WintersHelper.CalculateSmoothedTrendAndSeasonalValues(inputValues.ToList(), levelConstant, trendConstant,
        seasonalConstant, new List<double>(), new List<double>(), new List<double>());

      List<double> levelValues = initialCalculations.Item1.Skip(seasonLength - 1).ToList();
      List<double> trendValues = initialCalculations.Item2 .Skip(seasonLength - 1).ToList();
      List<double> seasonValues = initialCalculations.Item3;

      List<double> firstForecast = WintersHelper.FirstForecast(
        levelValues, trendValues, seasonValues, inputValues.Take(seasonLength - 1).ToList());

      double[] full = WintersHelper.Calculate(
        levelValues.Last(), trendValues.Last(), seasonValues.Skip(seasonValues.Count() - seasonLength - 1).First(), 1,
        amountOfPeriodsToCalculate, firstForecast).ToArray<double>();
      return ArrayBased.Split(full, amountOfPeriodsToCalculate);
    }
    public static (double[], double[]) CalculateBest(double[] inputValue, int amountOfPeriodsToCalculate)
    {
      List<(double[], double[])> all = new List<(double[], double[])>();
      for (double l = 0.1; l < 1.0; l+=0.1)
      {
        for (double t = 0.1; t < 1.0; t+=0.1)
        {
          for (double s = 0.1; s < 1.0; s+=0.1)
          {
            all.Add(Calculate(inputValue, l, t, s ,amountOfPeriodsToCalculate));
          }
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
