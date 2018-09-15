using System.Collections.Generic;
using System.Linq;

namespace Forecast.Method.AverageBased
{
  class Winters 
  {
    public static double[] Calculate(
      double[] inputValues, double levelConstant, double trendConstant,
      double seasonalConstant, int amountOfPeriodToCalculate)
    {
      (List<double>, List<double>, List<double>) initialCalculations =
        WintersHelper.CalculateSmoothedTrendAndSeasonalValues(
          inputValues.ToList(), levelConstant, trendConstant, seasonalConstant,
          new List<double>(), new List<double>(), new List<double>());

      return WintersHelper.Calculate(
        inputValues.ToList().Last(), new List<double>(), levelConstant,
        trendConstant, seasonalConstant, amountOfPeriodToCalculate, 1,
        initialCalculations.Item1.Last(), initialCalculations.Item2.Last(),
        initialCalculations.Item3.Last()).ToArray();
    }
  }
}
