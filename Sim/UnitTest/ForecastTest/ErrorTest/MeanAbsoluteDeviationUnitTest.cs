using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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

      double result =
        Forecast.Error.MeanAbsoluteDeviation.Calculation(input, forecast);
    }
  }
}
