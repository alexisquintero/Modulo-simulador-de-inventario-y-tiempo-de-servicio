using System.Linq;
using Forecast.Method.AverageBased;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utils.Exceptions;

namespace UnitTest.ForecastTest.MethodTest
{
  [TestClass]
  public class MovingAverageUnitTest
  {
    [TestMethod]
    public void Calculate_negativePeriods()
    {
      double[] input = new double[] { };
      try
      {
        MovingAverage.Calculate(input, -1, 1);
      }
      catch (NegativePeriodsToCalculate e)
      {
        StringAssert.Contains(e.Message, NegativePeriodsToCalculate.eMessage);
      }
    }
    public void Calculate_negativeMovinAverageTerms()
    {
      double[] input = new double[] { };
      try
      {
        MovingAverage.Calculate(input, 1, -1);
      }
      catch (NegativeMovingAverageTerms e)
      {
        StringAssert.Contains(e.Message, NegativeMovingAverageTerms.eMessage);
      }
    }
    [TestMethod]
    public void Calculate_zeroPeriods()
    {
      double[] input = new double[] { 200, 225, 245 };
      double[] forecast = MovingAverage.Calculate(input, 0, 1);

      CollectionAssert.AreEqual(input, forecast);
    }
    [TestMethod]
    public void Calculate_1Periods()
    {
      double[] input = new double[] { 275, 291, 307, 281, 295 };
      double[] forecast = MovingAverage.Calculate(input, 1, 5);
      double expectedValue = 289.8;

      Assert.AreEqual(expectedValue, forecast.Last(), 0.1);
    }
    [TestMethod]
    public void Calculate_nPeriods()
    {
      double[] input = new double[] { 654, 658, 665, 672, 673, 671, 693, 694, 701, 703, 702, 710, 712, 711, 728 };
      double[] forecast = MovingAverage.Calculate(input, 0, 3);
      double[] expectedValue = new double[] { 659, 665, 670, 672, 679, 686, 696, 699, 702, 705, 708, 711, 717 };

      CollectionAssert.AreEqual(forecast, expectedValue);
    }
  }
}
