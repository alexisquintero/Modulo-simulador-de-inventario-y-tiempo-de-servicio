using System.Collections.Generic;
using System.Linq;

namespace Forecast.Method.AverageBased
{
  public class Holt
  {
    public static double[] Calculate(
      double[] inputValue, int amountOfPeriodToCalculate,
      int movingAverageTerms, double dataSmoothingFactor,
      double trendSmoothingFactor) 
    {
      double initalSmoothedValue = inputValue[0];
      (List<double>, List<double>) auxValues = 
        HoltHelper.CalculteSmoothedAndTrendValues(
          inputValue.ToList(), dataSmoothingFactor, trendSmoothingFactor,
          new List<double>(), new List<double>());
      //Calculate forecast for periods which already have real values
      List<double> existantForecastedValues =
        HoltHelper.CalculateForecast(
          auxValues.Item1, auxValues.Item2, new List<double>());
      //Call function to calculate the 'n' next periods
      return HoltHelper.Calculate(
        inputValue.Last(), 0, amountOfPeriodToCalculate, new List<double>(),
        auxValues.Item1.Last(), dataSmoothingFactor, trendSmoothingFactor,
        auxValues.Item2.Last()).ToArray<double>();
    }
  }
}
