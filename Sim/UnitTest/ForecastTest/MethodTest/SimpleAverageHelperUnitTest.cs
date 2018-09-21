using System;
using System.Collections.Generic;
using System.Linq;
using Forecast.Method.AverageBased;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils.Exceptions;

namespace UnitTest.ForecastTest.MethodTest
{
  [TestClass]
  public class SimpleAverageHelperUnitTest
  {
    [TestMethod]
    public void Calculate_negativePeriods()
    {
      List<double> input = new List<double>();
      try
      {
        SimpleAverageHelper.Calculate(input, 0.0, -1);
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
      List<double> forecast = SimpleAverageHelper.Calculate(input, input.Sum(), 0);

      CollectionAssert.AreEqual(input, forecast);
    }
    [TestMethod]
    public void Calculate_1Periods()
    {
      List<double> input = new List<double>(new double[] { 100, 150, 200 });
      List<double> forecast = SimpleAverageHelper.Calculate(input, input.Sum(), 1);
      List<double> expectedValue = new List<double>(new double[] { 100, 150, 200, 150 });

      CollectionAssert.AreEqual(input, forecast);
    }
    public void Calculate_2Periods()
    {
      List<double> input = new List<double>(new double[] { 100, 150, 200 });
      List<double> forecast = SimpleAverageHelper.Calculate(input, input.Sum(), 2);
      List<double> expectedValue = new List<double>(new double[] { 100, 150, 200, 150, 150 });

      CollectionAssert.AreEqual(input, forecast);
    }
  }
}
