using System.Collections.Generic;
using System.Linq;

namespace Forecast.Method.AverageBased
{
  public class Winters 
  {
    public static double[] Calculate(double[] inputValues, double levelConstant,
      double trendConstant, double seasonalConstant,
      int amountOfPeriodToCalculate, int seasonLength = 4)
    {
      (List<double>, List<double>, List<double>) initialCalculations =
        WintersHelper.CalculateSmoothedTrendAndSeasonalValues(
          inputValues.ToList(), levelConstant, trendConstant, seasonalConstant,
          new List<double>(), new List<double>(), new List<double>());

      List<double> levelValues = initialCalculations.Item1
        .Skip(seasonLength - 1).ToList();
      List<double> trendValues = initialCalculations.Item2
        .Skip(seasonLength - 1).ToList();
      List<double> seasonValues = initialCalculations.Item3;

      List<double> firstForecast = WintersHelper.firstForecast(
        levelValues, trendValues, seasonValues,
        inputValues.Take(seasonLength - 1).ToList());

      return WintersHelper.Calculate(levelValues.Last(), trendValues.Last(),
        seasonValues.Skip(seasonValues.Count() - seasonLength - 1).First(), 1,
        amountOfPeriodToCalculate, firstForecast).ToArray<double>();
//      return WintersHelper.Calculate(
//        inputValues.ToList().Last(), new List<double>(), levelConstant,
//        trendConstant, seasonalConstant, amountOfPeriodToCalculate, 1,
//        initialCalculations.Item1.Last(), initialCalculations.Item2.Last(),
//        initialCalculations.Item3.Last()).ToArray();
    }
  }
}
