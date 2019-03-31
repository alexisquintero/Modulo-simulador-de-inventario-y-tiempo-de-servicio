using System.Collections.Generic;
using System.Linq;
using Utils;

namespace Forecast.Method.AverageBased
{
  public class SimpleAverage
  {
    public static string Name = "Promedio simple";
    public static (double[], double[]) Calculate(double[] inputValue, int amountOfPeriodsToCalculate)
    {
      Name = "Promedio simple";
      //Call function to calculate the 'n' next periods
      double[] full = SimpleAverageHelper.Calculate(
        inputValue.ToList(), inputValue.Sum(), amountOfPeriodsToCalculate).ToArray<double>();
      //Check if a value is less than 0
      if (full.Where(f => f < 0).Count() > 0) Name = "!!!" + Name;
      return ArrayBased.Split(full, amountOfPeriodsToCalculate);
    }
  }
}
