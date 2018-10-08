using Forecast.Method.AverageBased;
using Microsoft.VisualStudio.TestTools.UnitTesting;

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
      double[] expectedValue = new double[] { 500.0, 500.0, 450.5, 379.8,
        376.0, 390.5, 369.4, 304.6, 289.1, 295.1, 251.4, 202.8, 249.7,
        336.5, 337.7, 305.9, 381.0, 438.6, 432.7, 411.2, 476.8, 575.9,
        567.9, 527.4, 577.7 };

      CollectionAssert.AreEqual(expectedValue, forecastValue);
    }
  }
}
