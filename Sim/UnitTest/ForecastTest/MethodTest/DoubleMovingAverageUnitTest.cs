using System;
using System.Linq;
using Forecast.Method.AverageBased;
using Xunit;
using Utils.Exceptions;

namespace UnitTest.ForecastTest.MethodTest
{
  public class DoubleMovingAverageUnitTest
  {
    [Fact]
    public void Calculate_negativeMovinAverageTerms()
    {
      double[] input = new double[] { };
      try
      {
        DoubleMovingAverage.Calculate(input, 0, -1);
      }
      catch (NegativeMovingAverageTerms e)
      {
        Assert.Equal(e.Message, NegativeMovingAverageTerms.eMessage);
      }
    }
    [Fact]
    public void Calculate_1Periods()
    {
      double[] input = new double[] { 654, 658, 665, 672, 673, 671, 693, 694, 701, 703, 702, 710, 712, 711, 728 };
      double[] forecast = DoubleMovingAverage.Calculate(input, 1, 3);
      double[] expectedValue = new double[] { 681, 678, 690, 700, 714, 710, 708, 711, 714, 717, 727 };
      double[] simplifiedForecast = forecast.Select(d => Math.Round(d)).ToArray<double>();

      Assert.Equal(expectedValue, simplifiedForecast);
    }
  }
}
