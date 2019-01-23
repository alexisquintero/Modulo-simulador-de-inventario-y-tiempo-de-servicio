using System.Linq;
using Utils;
using Utils.Exceptions;

namespace Forecast.Method.AverageBased
{
  public class SimpleExponentialSmoothing
  {
    public static string Name = "Simple Exponential Smoothing";
    public static (double[], double[]) Calculate(double[] inputValue, double smoothingConstant)
    {
      if (0 == inputValue.Length) throw new EmptyParameterArray();
      double[] full = SimpleExponentialSmoothingHelper.Calculate(
        inputValue.ToList(), inputValue.Take(1).ToList<double>(), smoothingConstant).ToArray();
      return ArrayBased.Split(full, 1);
    }
  }
}
