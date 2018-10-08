using System.Collections.Generic;
using System.Linq;
using Forecast.Method.AverageBased;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils.Exceptions;

namespace UnitTest.ForecastTest.MethodTest
{
  [TestClass]
  public class MovingAverageHelperUnitTest
  {
    [TestMethod]
    public void Calculate_negativePeriods()
    {
      List<double> input = new List<double>();
      try
      {
        MovingAverageHelper.Calculate(input, -1, 1, 1, new List<double>());
      }
      catch (NegativePeriodsToCalculate e)
      {
        StringAssert.Contains(e.Message, NegativePeriodsToCalculate.eMessage);
      }
    }
    [TestMethod]
    public void Calculate_zeroPeriods()
    {
      List<double> input = new List<double>(new double[] { 200, 225, 245 });
      List<double> forecast = MovingAverageHelper.Calculate(input, 0, 1, 1, new List<double>());

      CollectionAssert.AreEqual(input, forecast);
    }
    [TestMethod]
    public void Calculate_1Periods()
    {
      List<double> input = new List<double>(new double[] { 275, 291, 307, 281, 295 });
      List<double> forecast = MovingAverageHelper.Calculate(input, 1, 1, 5, new List<double>());
      List<double> expectedValue = new List<double>(new double[] { 275, 291, 307, 281, 295, 289.8 });

      CollectionAssert.AreEqual(expectedValue, forecast);
    }
  }
}
