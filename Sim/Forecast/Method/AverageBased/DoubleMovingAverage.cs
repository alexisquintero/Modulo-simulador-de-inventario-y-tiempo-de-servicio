﻿using System.Collections.Generic;
using System.Linq;
using Utils.Exceptions;
using Utils;

namespace Forecast.Method.AverageBased
{
  public class DoubleMovingAverage
  {
    public static string Name = "Promedio móvil doble";
    //116 
    public static (double[], double[]) Calculate(double[] inputValue, int amountOfPeriodsToCalculate, int movingAverageTerms)
    {
      if (0 > movingAverageTerms) throw new NegativeMovingAverageTerms();
      if (inputValue.Length < movingAverageTerms) return (new List<double>(){-1}.ToArray(), new List<double>(){-1}.ToArray()); // throw new MovingAverageTermsBiggerThanInputSize();
      if (0 > amountOfPeriodsToCalculate) throw new NegativePeriodsToCalculate();
      Name = string.Format("Promedio móvil doble ({0})", movingAverageTerms);
      //Calculate first moving average
      double[] firstAverage = ArrayBased.Join(MovingAverage.Calculate(inputValue, movingAverageTerms));
      //Calculate second moving average
      double[] secondAverage = ArrayBased.Join(MovingAverage.Calculate(firstAverage, movingAverageTerms));
      //Remove first movingAverageTerms - 1 values since they are not use
      List<double> fixFirstAverage = firstAverage.ToList().Skip(movingAverageTerms - 1).ToList();
      //Call function to calculate the 'n' next periods
      double[] full = DoubleMovingAverageHelper.Calculate(
        fixFirstAverage, secondAverage.ToList(), new List<double>(),
        movingAverageTerms, 1, amountOfPeriodsToCalculate).ToArray<double>();
      //Check if a value is less than 0
      if (full.Where(f => f < 0).Count() > 0) Name = "!!!" + Name;
      return ArrayBased.Split(full, amountOfPeriodsToCalculate);
    }
  }
}
