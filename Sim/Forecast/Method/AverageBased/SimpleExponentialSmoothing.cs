using System.Collections.Generic;
using System.Linq;

namespace Forecast.Method.AverageBased
{
  public class SimpleExponentialSmoothing
  {
    public static double[] Calculate(double[] inputValue, double smoothingConstant)
    {
      return SimpleExponentialSmoothingHelper.Calculate(
        inputValue.ToList(), new List<double>(), smoothingConstant).ToArray();
    }
  }
}
