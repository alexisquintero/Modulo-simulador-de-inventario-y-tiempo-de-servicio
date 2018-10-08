using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils.Exceptions;

namespace UnitTest.ForecastTest.ErrorTest
{
  [TestClass]
  public class MeanAbsoluteDeviationUnitTest
  {
    [TestMethod]
    public void Calculation_emptyArrays()
    {
      double[] input = Array.Empty<double>();
      double[] forecast = Array.Empty<double>();

      try
      {
        double result =
          Forecast.Error.MeanAbsoluteDeviation.Calculation(input, forecast);
      }
      catch (EmptyParameterArray e)
      {
        StringAssert.Contains(e.Message, EmptyParameterArray.eMessage);
      }
    }
    [TestMethod]
    public void Calculation_differentSizeArrays()
    {
      double[] input = new double[] { 1 };
      double[] forecast = new double[] { 1, 2 };
      try
      {
        double result =
          Forecast.Error.MeanAbsoluteDeviation.Calculation(input, forecast);
      }
      catch (DifferentSizeArrays e)
      {
        StringAssert.Contains(e.Message, DifferentSizeArrays.eMessage);
      }
    }
    [TestMethod]
    public void Calculation_zeroOnRealInput()
    {
      double[] input = new double[] { 0 };
      double[] forecast = new double[] { 1 };
      try
      {
        double result =
          Forecast.Error.MeanAbsoluteDeviation.Calculation(input, forecast);
      }
      catch (ZeroInputArray e)
      {
        StringAssert.Contains(e.Message, ZeroInputArray.eMessage);
      }
    }
    [TestMethod]
    public void Calculation_BasicFlow()
    {
      double[] input = new double[] { 41.89, 37.5, 42.9 };
      double[] forecast = new double[] { 42.275, 42.275, 42.275 };
      double expected = 1.92833;

      double result =
        Forecast.Error.MeanAbsoluteDeviation.Calculation(input, forecast);

      Assert.AreEqual(expected, result, 0.001);
    }
  }
}
