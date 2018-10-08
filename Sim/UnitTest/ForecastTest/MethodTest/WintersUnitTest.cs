using Forecast.Method.AverageBased;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest.ForecastTest.MethodTest
{
  [TestClass]
  public class WintersUnitTest
  {
    double[] input = new double[] { 500, 350, 250, 400, 450, 350, 200, 300, 350,
      200, 150, 400, 550, 350, 250, 550, 550, 400, 350, 600, 750, 500, 400, 650,
      850, 600, 450, 700 };
    [TestMethod]
    public void Calculate()
    {
      double alpha = 0.4;
      double beta = 0.1;
      double gamma = 0.3;

      double[] forecast = Winters.Calculate(input, alpha, beta, gamma, 0);
      double[] expectedValue = new double[] { 563.257, 328.859, 222.565,
        375.344, 367.063, 249.255, 195.221, 315.576, 300.945, 202.255, 121.863,
        201.294, 292.949, 263.306, 211.335, 423.599, 532.584, 351.428, 264.999,
        568.284, 650.706, 468.626, 360.255, 712.712, 778.179, 521.917, 393.430,
        716.726 };

      CollectionAssert.AreEqual(forecast, expectedValue);
    }
  }
}
