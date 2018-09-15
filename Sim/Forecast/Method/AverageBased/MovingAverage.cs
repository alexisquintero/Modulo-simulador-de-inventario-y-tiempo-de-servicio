using System.Collections.Generic;
using System.Linq;

namespace Forecast.Method.AverageBased
{
  class MovingAverage
  {
    public static double[] Calculate(double[] inputValue, int amountOfPeriodToCalculate, int movingAverageTerms)
    {
      //Call function to calculate the 'n' next periods
      return MovingAverageHelper.Calculate(
        inputValue.ToList<double>(), amountOfPeriodToCalculate, 1,
        movingAverageTerms, new List<double>()).ToArray<double>();
    }
  }
}
