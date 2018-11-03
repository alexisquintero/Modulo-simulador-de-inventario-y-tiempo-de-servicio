using System.Collections.Generic;
using System.Linq;
using Forecast.Method.AverageBased;
using Xunit;
using Utils.Exceptions;

namespace UnitTest.ForecastTest.MethodTest
{
  public class SimpleAverageUnitTest
  {
    [Fact]
    public void Calculate_negativePeriods()
    {
      double[] input = new double[] { };
      try
      {
        SimpleAverage.Calculate(input, -1);
      }
      catch (NegativePeriodsToCalculate e)
      {
        Assert.Equal(e.Message, NegativePeriodsToCalculate.eMessage);
      }
    }
    [Fact]
    public void Calculate_zeroPeriods()
    {
      double[] input = new double[] { 200, 225, 245 };
      double[] forecast = SimpleAverage.Calculate(input, 0);

      Assert.Equal(input, forecast);
    }
    [Fact]
    public void Calculate_1Period()
    {
      double[] input = new double[] { 100, 150, 200 };
      double[] forecast = SimpleAverage.Calculate(input, 1);
      double[] expectedValue = new double[] { 100, 150, 200, 150 };

      Assert.Equal(expectedValue, forecast);
    }
  }
}
