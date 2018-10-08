using System.Collections.Generic;
using System.Linq;
using Utils.Exceptions;

namespace Forecast.Method.AverageBased
{
  public class MovingAverageHelper
  {
    public static List<double> Calculate(
      List<double> inputValue, int amountOfPeriodToCalculate, int index,
      int movingAverageTerms, List<double> outputValue)
    {
      if (0 > amountOfPeriodToCalculate) throw new NegativePeriodsToCalculate();
      if (0 == amountOfPeriodToCalculate) return inputValue;
      double period = 0;
      int newAmountOfPeriodToCalculate = amountOfPeriodToCalculate;
      int newIndex = index;
      //Return value if no need to calculate next periods
      if (index == amountOfPeriodToCalculate) return outputValue;
      else
      {
        //If input list has less elements than the moving average terms
        if (inputValue.Count < movingAverageTerms)
        {
          //Calculate period
          period = 
            (inputValue.Take(movingAverageTerms).Sum() + outputValue.Take(movingAverageTerms - inputValue.Count).Sum()) / movingAverageTerms;
          //Decrease by 1 the amount of periods to calculate since this this flow calculates a 'future' term
          newAmountOfPeriodToCalculate -= 1;
        }
        //If input list has more or equal elements than the moving average
        else
        {
          //Calculate period
          period = inputValue.Take(movingAverageTerms).Sum() / movingAverageTerms;
        }
        //Remove one element from input if possible
        if (inputValue.Count > 0) inputValue.RemoveAt(0);
        //Add period to values
        outputValue.Add(period);
        //Return new values
        return Calculate(
          inputValue, newAmountOfPeriodToCalculate, newIndex,
          movingAverageTerms, outputValue);
      }
    }
  }
}
