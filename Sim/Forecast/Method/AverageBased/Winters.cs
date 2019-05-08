using Forecast.Error;
using System.Collections.Generic;
using System.Linq;
using Utils;

namespace Forecast.Method.AverageBased
{
  public class Winters
  {
    public static string Name = "Winters";
    private static double SmallestError = double.MaxValue;
    private static decimal BestAlpha;
    private static decimal BestBeta;
    private static decimal BestGamma;
    private static (double[], double[]) BestResult;
    public static (double[], double[]) Calculate(double[] Y, decimal alpha, decimal beta, decimal gamma, int p, int s)
    {
      WintersHelper.Init(Y, s, alpha, beta, gamma);
      (double[], double[]) calculated = WintersHelper.Calculate(p, s);
      double mad = MeanAbsoluteDeviation.Calculation(Y, calculated.Item1);
      if(!double.IsNaN(mad) && mad < SmallestError)
      {
        SmallestError = mad;
        BestAlpha = alpha;
        BestBeta = beta;
        BestGamma = gamma;
        BestResult = calculated;
      }
      return calculated;
    }
    public static (double[], double[]) CalculateBest(double[] inputValue, int amountOfPeriodsToCalculate, int s)
    {
      SmallestError = double.MaxValue;
      for (decimal a = 0.1m; a < 1.0m; a+=0.1m) { for (decimal b = 0.1m; b < 1.0m; b+=0.1m) { for (decimal g = 0.1m; g < 1.0m; g+=0.1m) {
            Calculate(inputValue.Take(500).ToArray(), a, b, g, amountOfPeriodsToCalculate, s);
          } } }
      Name = string.Format("Winters | cte. de nivel: {0}, cte. de tendencia: {1}, cte. de estacionalidad: {2}", BestAlpha, BestBeta, BestGamma);
      //Check if a value is less than 0
      double[] full = ArrayBased.Join(BestResult);
      if (full.Where(f => f < 0).Count() > 0) Name = "!!!" + Name;
      return BestResult;
    }
  }
}
