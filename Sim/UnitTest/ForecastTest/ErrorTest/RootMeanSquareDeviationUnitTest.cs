using System;
using Xunit;
using Utils.Exceptions;

namespace UnitTest.ForecastTest.ErrorTest
{
  public class RootMeanSquareDeviationUnitTest
  {
    [Fact]
    public void Calculation_emptyArrays()
    {
      double[] input = Array.Empty<double>();
      double[] forecast = Array.Empty<double>();

      try
      {
        double result = Forecast.Error.RootMeanSquareDeviation.Calculation(input, forecast);
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
        double result = Forecast.Error.RootMeanSquareDeviation.Calculation(input, forecast);
      }
      catch (DifferentSizeArrays e)
      {
        Assert.Equal(e.Message, DifferentSizeArrays.eMessage);
      }
    }
    [Fact]
    public void Calculation_BasicFlow()
    {
      double[] input = new double[] { 41, 45, 49, 47, 44 };
      double[] forecast = new double[] { 43.6, 44.4, 45.2, 46, 46.8 };
      double expected = Math.Sqrt(6.08);

      double result = Forecast.Error.RootMeanSquareDeviation.Calculation(input, forecast);

      Assert.Equal(expected, result, 3);
    }
  }
}
