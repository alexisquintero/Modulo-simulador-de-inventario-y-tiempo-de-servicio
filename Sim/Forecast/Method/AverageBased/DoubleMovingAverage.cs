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
      return DoubleMovingAverageHelper.Calculate(
        fixFirstAverage, secondAverage.ToList(), new List<double>(),
        movingAverageTerms, 1, amountOfPeriodToCalculate).ToArray<double>();
    } 

  }
}
