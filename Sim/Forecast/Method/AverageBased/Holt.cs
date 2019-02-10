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
      double[] inputValue, int amountOfPeriodsToCalculate, double dataSmoothingFactor, double trendSmoothingFactor) 
    {
      SmallestError = double.MaxValue;
      double initalSmoothedValue = inputValue[0];
      (List<double>, List<double>) auxValues = HoltHelper.CalculteSmoothedAndTrendValues(
        inputValue.ToList(), dataSmoothingFactor, trendSmoothingFactor, new List<double>(), new List<double>());
      //Calculate forecast for periods which already have real values
      double[] full =
        HoltHelper.Calculate(auxValues.Item1, auxValues.Item2, inputValue.Take(1).ToList()).ToArray<double>();
      (double[], double[]) calculated = ArrayBased.Split(full, amountOfPeriodsToCalculate);
        double mad = MeanAbsoluteDeviation.Calculation(inputValue, calculated.Item1);
      if(!double.IsNaN(mad) && mad < SmallestError)
      {
        SmallestError = mad;
        BestDataSmoothing = dataSmoothingFactor;
        BestTrendSmoothing = trendSmoothingFactor;
        BestResult = calculated;
      }
      return calculated;
    }
    public static (double[], double[]) CalculateBest(double[] inputValue, int amountOfPeriodsToCalculate)
    {
      for (double d = 0.1; d < 1.0; d+=0.1) { for (double t = 0.1; t < 1.0; t+=0.1) {
          Calculate(inputValue, amountOfPeriodsToCalculate, d, t);
        } }
      Name = string.Format("Holt | cte. de nivel: {0}, cte. de tendencia: {1}", BestDataSmoothing, BestTrendSmoothing);
      return BestResult;
    }
  }
}
