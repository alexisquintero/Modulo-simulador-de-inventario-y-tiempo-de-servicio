using Forecast.Error;
using System.Collections.Generic;
using System.Linq;

namespace Forecast.Method.AverageBased
{
  public class Winters
  {
    public static string Name = "Winters";
    private static double SmallestError = double.MaxValue;
    private static double BestAlpha;
    private static double BestBeta;
    private static double BestGamma;
    private static (double[], double[]) BestResult;
    public static (double[], double[]) Calculate(double[] Y, double alpha, double beta, double gamma, int p, int s)
    {
      SmallestError = double.MaxValue;
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
      for (double a = 0.1; a < 1.0; a+=0.1) { for (double b = 0.1; b < 1.0; b+=0.1) { for (double g = 0.1; g < 1.0; g+=0.1) {
            Calculate(inputValue, a, b, g, amountOfPeriodsToCalculate, s);
          } } }
      Name = string.Format("Winters | cte. de nivel: {0}, cte. de tendencia: {1}, cte. de estacionalidad: {2}", BestAlpha, BestBeta, BestGamma);
      return BestResult;
    }
  }
}
