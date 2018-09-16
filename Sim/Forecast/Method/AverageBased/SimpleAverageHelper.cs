using System.Collections.Generic;

namespace Forecast.Method.AverageBased
{
  public class SimpleAverageHelper
  {
    public static List<double> Calculate(
      List<double> inputValue, double inputSum, int amountOfPeriodToCalculate)
    {
      //Return value if no need to calculate next periods
      if (0 == amountOfPeriodToCalculate) return inputValue;
      else
      {
        //Calculate next period
        double nextPeriod = inputSum / inputValue.Count;
        //Add next period to values
        inputValue.Add(nextPeriod);
        //Return new values
        return Calculate(inputValue, inputSum + nextPeriod, amountOfPeriodToCalculate - 1);
      }
    }
  }
}
