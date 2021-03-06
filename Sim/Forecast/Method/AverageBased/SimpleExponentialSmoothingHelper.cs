﻿using System.Collections.Generic;
using System.Linq;

namespace Forecast.Method.AverageBased
{
  public class SimpleExponentialSmoothingHelper
  {
    public static List<double> Calculate(
      List<double> realValues, List<double> output, double smoothingConstant)
    {
      if (0 == realValues.Count) return output;
      else
      {
        double forecastValue = output.Last() + smoothingConstant * (realValues.First() - output.Last());
        output.Add(forecastValue);
      }
      return Calculate(realValues.Skip(1).ToList(), output, smoothingConstant);
    }
  }
}
