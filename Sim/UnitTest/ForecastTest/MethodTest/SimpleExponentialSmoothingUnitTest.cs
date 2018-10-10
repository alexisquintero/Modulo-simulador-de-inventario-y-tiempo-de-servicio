using Forecast.Method.AverageBased;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace UnitTest.ForecastTest.MethodTest
{
  [TestClass]
  public class SimpleExponentialSmoothingUnitTest
  {
    double[] input = new double[] { 500, 350, 250, 400, 450, 350, 200, 300,
        350, 200, 150, 400, 550, 350, 250, 550, 550, 400, 350, 600, 750, 500,
        400, 650, 850 }; //Data from 4-7 ~70
    [TestMethod]
    public void Calculate_LowAlpha()
    {
      double alpha = 0.1;
      double[] forecast = SimpleExponentialSmoothing.Calculate(input, alpha);
      double[] expectedValue = new double[] { 500.0, 500.0, 485.0, 461.5, 455.4,
        454.8, 444.3, 419.9, 407.9, 402.1, 381.9, 358.7, 362.8, 381.6, 378.4,
        365.6, 384.0, 400.6, 400.5, 395.5, 415.9, 449.3, 454.4, 449.0, 469.1,
        507.2 };
     
      double[] simplifiedForecast = forecast.Select(f => Math.Round(f, 1)).ToArray<double>();

      CollectionAssert.AreEqual(simplifiedForecast, expectedValue);
    }
    [TestMethod]
    public void Calculate_HighAlpha()
    {
      double alpha = 0.6;
      double[] forecast = SimpleExponentialSmoothing.Calculate(input, alpha);
      double[] expectedValue = new double[] { 500.0, 500.0, 410.0, 314.0, 365.6,
        416.2, 376.5, 270.6, 288.2, 325.3, 250.1, 190.0, 316.0, 456.4, 392.6,
        307.0, 452.8, 511.1, 444.4, 387.8, 515.1, 656.0, 562.4, 465.0, 576.0,
        740.4 };
      
      double[] simplifiedForecast = forecast.Select(f => Math.Round(f, 1)).ToArray<double>();

      CollectionAssert.AreEqual(simplifiedForecast, expectedValue);
    }
  }
}
