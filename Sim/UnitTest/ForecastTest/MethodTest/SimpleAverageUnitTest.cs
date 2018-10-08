using System.Collections.Generic;
using System.Linq;
using Forecast.Method.AverageBased;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils.Exceptions;

namespace UnitTest.ForecastTest.MethodTest
{
  [TestClass]
  public class SimpleAverageUnitTest
  {
    [TestMethod]
    public void Calculate_negativePeriods()
    {
      double[] input = new double[] { };
      try
      {
        SimpleAverage.Calculate(input, -1);
      }
      catch (NegativePeriodsToCalculate e)
      {
        StringAssert.Contains(e.Message, NegativePeriodsToCalculate.eMessage);
      }
    }
    [TestMethod]
    public void Calculate_zeroPeriods()
    {
      double[] input = new double[] { 200, 225, 245 };
      double[] forecast = SimpleAverage.Calculate(input, 0);

      CollectionAssert.AreEqual(input, forecast);
    }
    [TestMethod]
    public void Calculate_1Period()
    {
      double[] input = new double[] { 100, 150, 200 };
      double[] forecast = SimpleAverage.Calculate(input, 1);
      double[] expectedValue = new double[] { 100, 150, 200, 150 };

      CollectionAssert.AreEqual(expectedValue, forecast);
    }
  }
}
