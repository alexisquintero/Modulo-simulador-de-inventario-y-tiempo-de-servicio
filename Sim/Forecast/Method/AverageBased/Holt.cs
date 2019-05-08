using Forecast.Error;
using System.Collections.Generic;
using System.Linq;
using Utils;

namespace Forecast.Method.AverageBased
{
  public class Holt
  {
    public static string Name = "Holt";
    private static double SmallestError = double.MaxValue;
    private static double BestDataSmoothing;
    private static double BestTrendSmoothing;
    private static (double[], double[]) BestResult;
    public static (double[], double[]) Calculate(
      double[] inputValue, int amountOfPeriodsToCalculate, decimal dataSmoothingFactor, decimal trendSmoothingFactor) 
    {
      SmallestError = double.MaxValue;
      double initalSmoothedValue = inputValue[0];
      (List<double>, List<double>) auxValues = HoltHelper.CalculteSmoothedAndTrendValues(
        inputValue.Select(d => (decimal)d).ToList(), dataSmoothingFactor, trendSmoothingFactor, new List<decimal>(), new List<decimal>());
      //Calculate forecast for periods which already have real values
      double[] full =
        HoltHelper.Calculate(auxValues.Item1, auxValues.Item2, inputValue.Take(1).ToList()).ToArray<double>();
      (double[], double[]) calculated = ArrayBased.Split(full, amountOfPeriodsToCalculate);
        double mad = MeanAbsoluteDeviation.Calculation(inputValue, calculated.Item1);
      if(!double.IsNaN(mad) && mad < SmallestError)
      {
        SmallestError = mad;
        BestDataSmoothing = (double)dataSmoothingFactor;
        BestTrendSmoothing = (double)trendSmoothingFactor;
        BestResult = calculated;
      }
      return calculated;
    }
    public static (double[], double[]) CalculateBest(double[] inputValue, int amountOfPeriodsToCalculate)
    {
      for (decimal d = 0.1m; d < 1.0m; d+=0.1m) { for (decimal t = 0.1m; t < 1.0m; t+=0.1m) {
          Calculate(inputValue.Take(500).ToArray(), amountOfPeriodsToCalculate, d, t);
        } }
      Name = string.Format("Holt | cte. de nivel: {0}, cte. de tendencia: {1}", BestDataSmoothing, BestTrendSmoothing);
      //Check if a value is less than 0
      double[] full = ArrayBased.Join(BestResult);
      if (full.Where(f => f < 0).Count() > 0) Name = "!!!" + Name;
      return BestResult;
    }
  }
}
