using System.Collections.Generic;
using System.Linq;
using Utils;

namespace Forecast.Method.AverageBased
{
  public class SimpleAverage
  {
    public static string Name = "Simple Average";
    public static (double[], double[]) Calculate(double[] inputValue, int amountOfPeriodsToCalculate)
    {
      //Call function to calculate the 'n' next periods
      double[] full = SimpleAverageHelper.Calculate(
        inputValue.ToList(), inputValue.Sum(), amountOfPeriodsToCalculate).ToArray<double>();
      return ArrayBased.Split(full, amountOfPeriodsToCalculate);
    }
    
  }
}
