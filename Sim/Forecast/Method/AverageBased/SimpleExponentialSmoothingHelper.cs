using System.Collections.Generic;
using System.Linq;

namespace Forecast.Method.AverageBased
{
  class SimpleExponentialSmoothingHelper
  {
    public static List<double> Calculate(
      List<double> realValues, List<double> output, double smoothingConstant)
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
      return Calculate(realValues.Skip(1).ToList(), output, smoothingConstant);
    }
  }
}
