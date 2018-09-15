using System.Collections.Generic;
using System.Linq;

namespace Forecast.Method.AverageBased
{
  class SimpleAverageHelper
  {
    public static List<double> Calculate(List<double> inputValue, int amountOfPeriodToCalculate)
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
