using System.Collections.Generic;
using System.Linq;
using Utils.Exceptions;

namespace Forecast.Method.AverageBased
{
  public class MovingAverage
  {
    public static double[] Calculate(double[] inputValue, int movingAverageTerms)
    {
      if (0 > movingAverageTerms) throw new NegativeMovingAverageTerms();
      if (inputValue.Length < movingAverageTerms) throw new MovingAverageTermsBiggerThanInputSize();
      //Call function to calculate the 'n' next periods
      return MovingAverageHelper.Calculate(
        inputValue.ToList<double>(), movingAverageTerms, new List<double>()).ToArray<double>();
    }
  }
}
