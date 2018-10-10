using System.Collections.Generic;
using System.Linq;
using Utils.Exceptions;

namespace Forecast.Method.AverageBased
{
  public class SimpleExponentialSmoothing
  {
    public static double[] Calculate(double[] inputValue, double smoothingConstant)
    {
      if (0 == inputValue.Length) throw new EmptyParameterArray();
      return SimpleExponentialSmoothingHelper.Calculate(
        inputValue.ToList(), inputValue.Take(1).ToList<double>(), smoothingConstant).ToArray();
    }
  }
}
