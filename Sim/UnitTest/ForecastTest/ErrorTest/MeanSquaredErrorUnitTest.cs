using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils.Exceptions;

namespace UnitTest.ForecastTest.ErrorTest
{
  [TestClass]
  public class MeanSquaredErrorUnitTest
  {
    [TestMethod]
    public void Calculation_emptyArrays()
    {
      double[] input = Array.Empty<double>();
      double[] forecast = Array.Empty<double>();

      try
      {
        double result =
          Forecast.Error.MeanSquaredError.Calculation(input, forecast);
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
          Forecast.Error.MeanSquaredError.Calculation(input, forecast);
      }
      catch (DifferentSizeArrays e)
      {
        StringAssert.Contains(e.Message, DifferentSizeArrays.eMessage);
      }
    }
    [TestMethod]
    public void Calculation_BasicFlow()
    {
      double[] input = new double[] { 41, 45, 49, 47, 44 };
      double[] forecast = new double[] { 43.6, 44.4, 45.2, 46, 46.8 };
      double expected = 6.08;

      double result =
        Forecast.Error.MeanSquaredError.Calculation(input, forecast);

      Assert.AreEqual(expected, result, 0.001);
    }
  }
}