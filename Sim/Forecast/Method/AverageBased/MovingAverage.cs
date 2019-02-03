using System.Collections.Generic;
using System.Linq;
using Utils;
using Utils.Exceptions;

namespace Forecast.Method.AverageBased
{
  public class MovingAverage
  {
    public static string Name = "Moving Average";
    public static (double[], double[]) Calculate(double[] inputValue, int movingAverageTerms)
    {
      if (0 > movingAverageTerms) throw new NegativeMovingAverageTerms();
      if (inputValue.Length < movingAverageTerms) throw new MovingAverageTermsBiggerThanInputSize();
      Name += string.Format(" ({0})", movingAverageTerms);
      //Call function to calculate the 'n' next periods
      double[] full = MovingAverageHelper.Calculate(
        inputValue.ToList(), movingAverageTerms, new List<double>()).ToArray<double>();
      return ArrayBased.Split(full, 1);
    }
  }
}
