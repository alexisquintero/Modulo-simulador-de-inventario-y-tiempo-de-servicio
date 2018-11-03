using System;
using System.Linq;
using Forecast.Method.AverageBased;
using Xunit;
using Utils.Exceptions;

namespace UnitTest.ForecastTest.MethodTest
{
  public class MovingAverageUnitTest
  {
    [Fact]
    public void Calculate_negativeMovinAverageTerms()
    {
      double[] input = new double[] { };
      try
      {
        MovingAverage.Calculate(input, -1);
      }
      catch (NegativeMovingAverageTerms e)
      {
        Assert.Equal(e.Message, NegativeMovingAverageTerms.eMessage);
      }
    }
    [Fact]
    public void Calculate_1Periods()
    {
      double[] input = new double[] { 275, 291, 307, 281, 295 };
      double[] forecast = MovingAverage.Calculate(input, 5);
      double expectedValue = 289.8;

      Assert.Equal(expectedValue, forecast.Last(), 1);
    }
    [Fact]
    public void Calculate_0Periods()
    {
      double[] input = new double[] { 654, 658, 665, 672, 673, 671, 693, 694, 701, 703, 702, 710, 712, 711, 728 };
      double[] forecast = MovingAverage.Calculate(input, 3);
      double[] expectedValue = new double[] { 659, 665, 670, 672, 679, 686, 696, 699, 702, 705, 708, 711, 717 };
      double[] simplifiedForecast = forecast.Select(d => Math.Round(d)).ToArray<double>();

      Assert.Equal(expectedValue, simplifiedForecast);
    }
  }
}
