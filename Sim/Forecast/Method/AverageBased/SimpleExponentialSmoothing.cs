using System.Linq;
using Utils;
using Utils.Exceptions;

namespace Forecast.Method.AverageBased
{
  public class SimpleExponentialSmoothing
  {
    public static string Name = "Suavización exponencial simple";
    public static (double[], double[]) Calculate(double[] inputValue, double smoothingConstant)
    {
      if (0 == inputValue.Length) throw new EmptyParameterArray();
      Name = string.Format("Suavización exponencial simple | cte. de suavización: {0}", smoothingConstant);
      double[] full = SimpleExponentialSmoothingHelper.Calculate(
        inputValue.ToList(), inputValue.Take(1).ToList<double>(), smoothingConstant).ToArray();
      return ArrayBased.Split(full, 1);
    }
  }
}
