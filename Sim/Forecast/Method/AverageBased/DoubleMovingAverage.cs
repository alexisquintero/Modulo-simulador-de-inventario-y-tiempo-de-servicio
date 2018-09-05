using System.Collections.Generic;
using System.Linq;

namespace Forecast.Method.AverageBased
{
  class DoubleMovingAverage
  {
    //116 
    public static double[] Calculate(double[] inputValue, int amountOfPeriodToCalculate, int movingAverageTerms)
    {
      //Calculate first moving average
      double[] firstAverage = MovingAverage.Calculate(inputValue, amountOfPeriodToCalculate, movingAverageTerms);
      //Calculate second moving average
      double[] secondAverage = MovingAverage.Calculate(firstAverage, amountOfPeriodToCalculate, movingAverageTerms);
      //Remove first movingAverageTerms - 1 values since they are not use
      List<double> fixFirstAverage = firstAverage.ToList().Skip(movingAverageTerms - 1).ToList();
      //Call function to calculate the 'n' next periods
      return Calculate(fixFirstAverage, secondAverage.ToList(), new List<double>(), movingAverageTerms, 1, amountOfPeriodToCalculate).ToArray<double>();
    } 
    private static List<double> Calculate(List<double> firstAverage, List<double> secondAverage, List<double> outputValue, int movingAverageTerms, int index, int amountOfPeriodToCalculate)
    {
      if (index > amountOfPeriodToCalculate) return outputValue;
      double at = 2 * firstAverage.First() - secondAverage.First();
      double bt = 2 / (movingAverageTerms - 1) * (firstAverage.First() - secondAverage.First());
      double forecastValue = at + bt * index;
      outputValue.Add(forecastValue);
      if (1 == firstAverage.Count)
      {
        return Calculate(firstAverage, secondAverage, outputValue, movingAverageTerms, index++, amountOfPeriodToCalculate);
      }
      else
      {
        firstAverage.RemoveAt(0);
        secondAverage.RemoveAt(0);
        return Calculate(firstAverage, secondAverage, outputValue, movingAverageTerms, index, amountOfPeriodToCalculate);
      }
    }
  }
}
