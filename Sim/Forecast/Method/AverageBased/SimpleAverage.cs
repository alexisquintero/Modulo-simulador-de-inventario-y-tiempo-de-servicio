using System.Collections.Generic;
using System.Linq;

namespace Forecast.Method.AverageBased
{
  class SimpleAverage
  {
    public static double[] Calculate(double[] inputValue, int amountOfPeriodToCalculate)
    {
      //Call function to calculate the 'n' next periods
      return Calculate(inputValue.ToList<double>(), amountOfPeriodToCalculate).ToArray<double>();
    }
    private static List<double> Calculate(List<double> inputValue, int amountOfPeriodToCalculate)
    {
      //Return value if no need to calculate next periods
      if (0 == amountOfPeriodToCalculate) return inputValue;
      else
      {
        //Calculate next period
        double nextPeriod = inputValue.Sum() / inputValue.Count;
        //Add next period to values
        inputValue.Add(nextPeriod);
        //Return new values
        return Calculate(inputValue, amountOfPeriodToCalculate - 1);
      }
    }
  }
}
