using System.Collections.Generic;
using System.Linq;

namespace Forecast.Method.AverageBased
{
  class SimpleExponentialSmoothing
  {
    public static double[] Calculate(double[] inputValue, double smoothingConstant)
    {
      return Calculate(inputValue.ToList(), new List<double>(), smoothingConstant).ToArray();
    }
    private static List<double> Calculate(List<double> realValues, List<double> output, double smoothingConstant)
    {
      if (0 == realValues.Count) return output;
      else if (0 == output.Count)
      {
        output.Add(realValues.First());
      }
      else
      {
        double forecastValue = output.Last() + smoothingConstant * (realValues.First() - output.Last());
        output.Add(forecastValue);
      }
      realValues.RemoveAt(0);
      return Calculate(realValues, output, smoothingConstant);
    }
  }
}
