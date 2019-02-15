using System.Collections.Generic;
using System.Linq;
using Utils;
using Utils.Exceptions;

namespace Forecast.Method.AverageBased
{
  public class MovingAverage
  {
    public static string Name = "Promedio móvil";
    public static (double[], double[]) Calculate(double[] inputValue, int movingAverageTerms)
    {
      if (0 > movingAverageTerms) throw new NegativeMovingAverageTerms();
      if (inputValue.Length < movingAverageTerms) return (new double[0], new double[0]);// throw new MovingAverageTermsBiggerThanInputSize();
      Name = string.Format("Promedio móvil ({0})", movingAverageTerms);
      //Call function to calculate the 'n' next periods
      double[] full = MovingAverageHelper.Calculate(
        inputValue.ToList(), movingAverageTerms, new List<double>()).ToArray<double>();
      return ArrayBased.Split(full, 1);
    }
  }
}
