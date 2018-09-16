using System.Collections.Generic;
using System.Linq;

namespace Forecast.Method.AverageBased
{
  public class SimpleAverage
  {
    public static double[] Calculate(double[] inputValue, int amountOfPeriodToCalculate)
    {
      //Call function to calculate the 'n' next periods
      return SimpleAverageHelper.Calculate(
        inputValue.ToList<double>(), inputValue.Sum(), amountOfPeriodToCalculate).ToArray<double>();
    }
    
  }
}
