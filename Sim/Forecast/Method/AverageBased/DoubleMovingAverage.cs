﻿using System.Collections.Generic;
using System.Linq;
using Utils.Exceptions;

namespace Forecast.Method.AverageBased
{
  public class DoubleMovingAverage
  {
    //116 
    public static double[] Calculate(double[] inputValue, int amountOfPeriodToCalculate, int movingAverageTerms)
    {
      if (0 > movingAverageTerms) throw new NegativeMovingAverageTerms();
      if (inputValue.Length < movingAverageTerms) throw new MovingAverageTermsBiggerThanInputSize();
      if (0 > amountOfPeriodToCalculate) throw new NegativePeriodsToCalculate();
      //Calculate first moving average
      double[] firstAverage = MovingAverage.Calculate(inputValue, movingAverageTerms);
      //Calculate second moving average
      double[] secondAverage = MovingAverage.Calculate(firstAverage, movingAverageTerms);
      //Remove first movingAverageTerms - 1 values since they are not use
      List<double> fixFirstAverage = firstAverage.ToList().Skip(movingAverageTerms - 1).ToList();
      //Call function to calculate the 'n' next periods
      return DoubleMovingAverageHelper.Calculate(
        fixFirstAverage, secondAverage.ToList(), new List<double>(),
        movingAverageTerms, 1, amountOfPeriodToCalculate).ToArray<double>();
    } 

  }
}
