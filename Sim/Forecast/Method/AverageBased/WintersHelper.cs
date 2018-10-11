using System.Collections.Generic;
using System.Linq;

namespace Forecast.Method.AverageBased
{
  public class WintersHelper
  {
    //    public static List<double> Calculate(
    //     double lastRealValue, List<double> outputValues, double levelConstant,
    //     double trendConstant, double seasonalConstant,
    //     int amountOfPeriodToCalculate, int index, double smoothedValue,
    //     double trendValue, double seasonalValue)
    //    {
    //      if (0 == amountOfPeriodToCalculate) return outputValues;
    //      else
    //      {
    //        //Calculate new smoothed value
    //        double newSmoothedValue =
    //          SmoothedValue(
    //            levelConstant, lastRealValue, seasonalValue, smoothedValue,
    //            trendValue);
    //        //Calculate new trend value
    //        double newTrendValue =
    //          TrendValue(trendConstant, newSmoothedValue, smoothedValue,
    //          trendValue);
    //        //Calculate new seasonal value
    //        double newSeasonalValue =
    //          SeasonalValue(
    //            seasonalConstant, lastRealValue, newSmoothedValue, seasonalValue);
    //        //Add new forecasted value
    //        outputValues.Add(
    //          Forecast(newSmoothedValue, index, newTrendValue, newSeasonalValue));
    //        return Calculate(
    //          lastRealValue, outputValues, levelConstant, trendConstant,
    //          seasonalConstant, --amountOfPeriodToCalculate, index++,
    //          newSmoothedValue, newTrendValue, newSeasonalValue);
    //      }
    //    }
    public static List<double> Calculate(double level, double trend,
      double season, int index, int amountOfPeriodsToCalculate,
      List<double> output)
    {
      if (index > amountOfPeriodsToCalculate) return output;
      else
      {
        double forecastValue = Forecast(level, index, trend, season);
        output.Add(forecastValue);
        return Calculate(level, trend, season, ++index, amountOfPeriodsToCalculate, output);
      }
    }
    public static double SmoothedValue(
      double levelConstant, double realValue, double previousSeasonalValue,
      double previousSmoothedValue, double previousTrendValue)
    {
      return levelConstant * realValue / previousSeasonalValue + (1 - levelConstant) * (previousSmoothedValue + previousTrendValue);
    }
    public static double TrendValue(
      double trendConstant, double smoothedValue, double previousSmoothedValue,
      double previousTrendValue)
    {
      return trendConstant * (smoothedValue - previousSmoothedValue) + (1 - trendConstant) * previousTrendValue;
    }
    public static double SeasonalValue(
      double seasonalConstant, double realValue, double smoothedValue,
      double previousSeasonalValue)
    {
      return seasonalConstant * realValue / smoothedValue + (1 - seasonalConstant) * previousSeasonalValue;
    }
    public static List<double> firstForecast(List<double> levels,
      List<double> trends, List<double> seasons, List<double> output)
    {
      if (1 == levels.Count()) return output;
      else
      {
        double forecastValue = Forecast(levels.First(), 1, trends.First(), seasons.First());
        output.Add(forecastValue);
        return firstForecast(levels.Skip(1).ToList(), trends.Skip(1).ToList(),
          seasons.Skip(1).ToList(), output);
      }
    }
    public static double Forecast(
      double smoothedValue, int index, double trendValue, double seasonalValue)
    {
      return (smoothedValue + index * trendValue) * seasonalValue;
    }
    public static (List<double>, List<double>, List<double>) 
      CalculateSmoothedTrendAndSeasonalValues(
        List<double> inputValues, double levelConstant, double trendConstant,
        double seasonalConstant, List<double> smoothedValues,
        List<double> trendValues, List<double> seasonalValues,
        bool firstRun = true)
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
        double newSmoothedValue = SmoothedValue(levelConstant,
          inputValues.First(), seasonalValues.Last(), smoothedValues.Last(),
          trendValues.Last());
        double newTrendValue = TrendValue(trendConstant, newSmoothedValue,
          smoothedValues.Last(), trendValues.Last());
        double newSeasonalValue = SeasonalValue(seasonalConstant,
          inputValues.First(), smoothedValues.Last(), seasonalValues.Last());
        smoothedValues.Add(newSmoothedValue);
        trendValues.Add(newTrendValue);
        seasonalValues.Add(newSeasonalValue);
      }
      return CalculateSmoothedTrendAndSeasonalValues(
        inputValues.Skip(1).ToList(), levelConstant, trendConstant,
        seasonalConstant, smoothedValues, trendValues, seasonalValues,
        false);
    }
  }
}
