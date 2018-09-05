using System.Collections.Generic;
using System.Linq;

namespace Forecast.Method.AverageBased
{
  class Winters 
  {
    public static double[] Calculate(double[] inputValues, double levelConstant, double trendConstant, double seasonalConstant, int amountOfPeriodToCalculate)
    {
      (List<double>, List<double>, List<double>) initialCalculations =
        CalculateSmoothedTrendAndSeasonalValues(inputValues.ToList(), levelConstant, trendConstant, seasonalConstant, new List<double>(), new List<double>(), new List<double>());
      return Calculate(
        inputValues.ToList().Last(), new List<double>(), levelConstant, trendConstant, seasonalConstant, amountOfPeriodToCalculate, 1, initialCalculations.Item1.Last(), initialCalculations.Item2.Last(), initialCalculations.Item3.Last()).ToArray();
    }
    private static List<double> Calculate(
      double lastRealValue, List<double> outputValues, double levelConstant, double trendConstant, double seasonalConstant, int amountOfPeriodToCalculate, int index, double smoothedValue, double trendValue, double seasonalValue)
    {
      if (0 == amountOfPeriodToCalculate) return outputValues;
      else
      {
        //Calculate new smoothed value
        double newSmoothedValue = SmoothedValue(levelConstant, lastRealValue, seasonalValue, smoothedValue, trendValue);
        //Calculate new trend value
        double newTrendValue = TrendValue(trendConstant, newSmoothedValue, smoothedValue, trendValue);
        //Calculate new seasonal value
        double newSeasonalValue = SeasonalValue(seasonalConstant, lastRealValue, newSmoothedValue, seasonalValue);
        //Add new forecasted value
        outputValues.Add(Forecast(newSmoothedValue, index, newTrendValue, newSeasonalValue));
        return Calculate(lastRealValue, outputValues, levelConstant, trendConstant, seasonalConstant, amountOfPeriodToCalculate--, index++, newSmoothedValue, newTrendValue, newSeasonalValue);
      }
    }
    private static double SmoothedValue(double levelConstant, double realValue, double previousSeasonalValue, double previousSmoothedValue, double previousTrendValue)
    {
      return levelConstant * realValue / previousSeasonalValue + (1 - levelConstant) * (previousSmoothedValue + previousTrendValue);
    }
    private static double TrendValue(double trendConstant, double smoothedValue, double previousSmoothedValue, double previousTrendValue)
    {
      return trendConstant * (smoothedValue - previousSmoothedValue) + (1 - trendConstant) * previousTrendValue;
    }
    private static double SeasonalValue(double seasonalConstant, double realValue, double smoothedValue, double previousSeasonalValue)
    {
      return seasonalConstant * realValue / smoothedValue + (1 - seasonalConstant) * previousSeasonalValue;
    }
    private static double Forecast(double smoothedValue, int index, double trendValue, double seasonalValue)
    {
      return (smoothedValue + index * trendValue) * seasonalValue;
    }
    private static (List<double>, List<double>, List<double>) CalculateSmoothedTrendAndSeasonalValues(
      List<double> inputValues, double levelConstant, double trendConstant, double seasonalConstant, List<double> smoothedValues, List<double> trendValues, List<double> seasonalValues, bool firstRun = true)
    {
      //First smoothed value equals first real value
      //First trend value equals zero
      //First seasonal value equals 1.0
      if (0 == inputValues.Count) return (smoothedValues, trendValues, seasonalValues);
      if (firstRun)
      {
        smoothedValues.Add(inputValues.First());
        trendValues.Add(0.0);
        seasonalValues.Add(1.0);
      }
      else
      {
        double newSmoothedValue = SmoothedValue(levelConstant, inputValues.First(), seasonalValues.Last(), smoothedValues.Last(), trendValues.Last());
        smoothedValues.Add(newSmoothedValue);
        trendValues.Add(TrendValue(trendConstant, newSmoothedValue, smoothedValues.Last(), trendValues.Last()));
        seasonalValues.Add(SeasonalValue(seasonalConstant, inputValues.First(), smoothedValues.Last(), seasonalValues.Last()));
      }
      inputValues.RemoveAt(0);
      return CalculateSmoothedTrendAndSeasonalValues(inputValues, levelConstant, trendConstant, seasonalConstant, smoothedValues, trendValues, seasonalValues, false);
    }
  }
}
