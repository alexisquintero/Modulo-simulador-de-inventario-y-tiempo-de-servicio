using System.Collections.Generic;
using System.Linq;

namespace Forecast.Method.AverageBased
{
  public class Holt
  {
    public static double[] Calculate(
      double[] inputValue, int amountOfPeriodToCalculate, double dataSmoothingFactor, double trendSmoothingFactor) 
    {
      double initalSmoothedValue = inputValue[0];
      (List<double>, List<double>) auxValues = 
        HoltHelper.CalculteSmoothedAndTrendValues(inputValue.ToList(),
        dataSmoothingFactor, trendSmoothingFactor, new List<double>(),
        new List<double>());
      //Calculate forecast for periods which already have real values
      return HoltHelper.Calculate(auxValues.Item1, auxValues.Item2,
        inputValue.Take(1).ToList<double>()).ToArray<double>();
    }
  }
}
