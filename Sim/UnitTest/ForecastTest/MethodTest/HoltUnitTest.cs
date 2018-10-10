using Forecast.Method.AverageBased;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace UnitTest.ForecastTest.MethodTest
{
  [TestClass]
  public class HoltUnitTest
  {
    double[] input = new double[] { 500, 350, 250, 400, 450, 350, 200, 300,
      350, 200, 150, 400, 550, 350, 250, 550, 550, 400, 350, 600, 750, 500,
      400, 650, 850 }; //Data from 4-8 ~73
    [TestMethod]
    public void Calculate()
    {
      double alpha = 0.3;
      double beta = 0.1;
      double[] forecastValue = Holt.Calculate(input, 1, alpha, beta);
      double[] expectedValue = new double[] { 500.0, 500.7, 451.0, 380.1,
        376.1, 390.6, 369.4, 304.6, 289.0, 295.0, 251.3, 202.7, 249.6,
        336.4, 337.6, 305.8, 380.9, 438.5, 432.7, 411.1, 476.7, 575.8,
        567.9, 527.3, 577.6, 681 };
      double[] simplifiedForecast = forecastValue.Select(f => Math.Round(f, 1)).ToArray<double>();

      CollectionAssert.AreEqual(expectedValue, simplifiedForecast);
    }
  }
}
