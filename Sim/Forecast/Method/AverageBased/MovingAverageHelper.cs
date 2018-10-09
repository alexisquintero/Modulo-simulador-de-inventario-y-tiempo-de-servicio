using System.Collections.Generic;
using System.Linq;

namespace Forecast.Method.AverageBased
{
  public class MovingAverageHelper
  {
    public static List<double> Calculate(
      List<double> inputValue, int movingAverageTerms, List<double> outputValue)
    {
      if (inputValue.Count() < movingAverageTerms) return outputValue;
      else
      {
        outputValue.Add(inputValue.Take(movingAverageTerms).Sum() / movingAverageTerms);
        return Calculate(inputValue.Skip(1).ToList<double>(), movingAverageTerms, outputValue);
      }
    }
  }
}
