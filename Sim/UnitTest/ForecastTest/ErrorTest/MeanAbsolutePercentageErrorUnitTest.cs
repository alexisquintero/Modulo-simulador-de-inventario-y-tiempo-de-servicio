using System;
using Xunit;
using Utils.Exceptions;

namespace UnitTest.ForecastTest.ErrorTest
{
  public class MeanAbsolutePercentageErrorUnitTest
  {
    [Fact]
    public void Calculation_emptyArrays()
    {
      double[] input = Array.Empty<double>();
      double[] forecast = Array.Empty<double>();

      try
      {
        double result = Forecast.Error.MeanAbsolutePercentageError.Calculation(input, forecast);
      }
      catch (EmptyParameterArray e)
      {
        Assert.Equal(e.Message, EmptyParameterArray.eMessage);
      }
    }
    [Fact]
    public void Calculation_differentSizeArrays()
    {
      double[] input = new double[] { 1 };
      double[] forecast = new double[] { 1, 2 };
      try
      {
        double result = Forecast.Error.MeanAbsolutePercentageError.Calculation(input, forecast);
      }
      catch (DifferentSizeArrays e)
      {
        Assert.Equal(e.Message, DifferentSizeArrays.eMessage);
      }
    }
    [Fact]
    public void Calculation_BasicFlow()
    {
      double[] input = new double[] { 41.89, 37.5, 42.9 };
      double[] forecast = new double[] { 42.275, 42.275, 42.275 };
      double expected = 0.05036;

      double result = Forecast.Error.MeanAbsolutePercentageError.Calculation(input, forecast);

      Assert.Equal(expected, result, 3);
    }
  }
}
