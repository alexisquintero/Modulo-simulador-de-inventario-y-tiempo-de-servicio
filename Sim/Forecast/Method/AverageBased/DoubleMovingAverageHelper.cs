﻿using System.Collections.Generic;
using System.Linq;

namespace Forecast.Method.AverageBased
{
  class DoubleMovingAverageHelper
  {
    public static List<double> Calculate(
      List<double> firstAverage, List<double> secondAverage, List<double> outputValue, int movingAverageTerms, int index, int amountOfPeriodToCalculate)
    {
      if (secondAverage.Count == 0 || firstAverage.Count == 0) return new List<double>();
      if (index > amountOfPeriodToCalculate) return outputValue;
      double at = 2 * firstAverage.First() - secondAverage.First();
      double bt = 2 / (movingAverageTerms - 1) * (firstAverage.First() - secondAverage.First());
      double forecastValue = at + bt * index;
      outputValue.Add(forecastValue);
      if (1 == firstAverage.Count)
      {
        return Calculate(
          firstAverage, secondAverage, outputValue, movingAverageTerms, ++index, amountOfPeriodToCalculate);
      }
      else
      {
        return Calculate(
          firstAverage.Skip(1).ToList(), secondAverage.Skip(1).ToList(),
          outputValue, movingAverageTerms, index, amountOfPeriodToCalculate);
      }
    }
  }
}
