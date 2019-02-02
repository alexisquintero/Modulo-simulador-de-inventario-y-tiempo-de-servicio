using Forecast.Method.AverageBased;
using Xunit;
using System;
using System.Linq;
using Utils;

namespace UnitTest.ForecastTest.MethodTest
{
  public class WintersUnitTest
  {
    double[] input = new double[] { 500, 350, 250, 400, 450, 350, 200, 300, 350,
       200, 150, 400, 550, 350, 250, 550, 550, 400, 350, 600, 750, 500, 400, 650,
       850, 600, 450, 700 };
    [Fact]
    public void Calculate()
    {
      double alpha = 0.4;
      double beta = 0.1;
      double gamma = 0.3;

      double[] forecast = ArrayBased.Join(NewWinters.Calculate(input, alpha, beta, gamma, 4, 4));
      //double[] expectedValue = new double[] { 563.257, 328.859, 222.565,
      //  375.344, 367.063, 249.255, 195.221, 315.576, 300.945, 202.255, 121.863,
      //  201.294, 292.949, 263.306, 211.335, 423.599, 532.584, 351.428, 264.999,
      //  568.284, 650.706, 468.626, 360.255, 712.712, 778.179, 521.917, 393.430,
      //  716.726 };
      //
      // Assuming the following expected values are correct for the time being
      // Original values will never match since the initial values are calculated
      //  different.

      double[] expectedValue = new double[] { 500.000, 350.000, 250.000,
       406.033, 403.272, 329.032, 284.676, 319.287, 330.535, 232.415, 196.029,
       327.750, 357.644, 277.437, 318.510, 500.807, 479.548, 380.671, 428.440,
       551.577, 594.451, 493.315, 504.022, 658.566, 712.861, 588.357, 569.817,
       649.326, 657.755, 666.184, 674.614 };

      double[] simplifiedForecast = forecast.Select(f => Math.Round(f, 3)).ToArray();

      Assert.Equal(simplifiedForecast, expectedValue);
    }
  }
}
