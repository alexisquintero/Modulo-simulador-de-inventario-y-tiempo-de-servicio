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
      (List<double>, List<double>) auxValues = 
        HoltHelper.CalculteSmoothedAndTrendValues(inputValue.ToList(),
        dataSmoothingFactor, trendSmoothingFactor, new List<double>(),
        new List<double>());
      //Calculate forecast for periods which already have real values
      double[] full = HoltHelper.Calculate(auxValues.Item1, auxValues.Item2,
        inputValue.Take(1).ToList<double>()).ToArray<double>();
      return ArrayBased.Split(full, amountOfPeriodsToCalculate);
    }
  }
}
