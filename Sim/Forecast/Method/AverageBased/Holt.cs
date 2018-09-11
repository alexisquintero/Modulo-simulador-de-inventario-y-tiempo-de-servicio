using System.Collections.Generic;
using System.Linq;

namespace Forecast.Method.AverageBased
{
  class Holt
  {
    public static double[] Calculate(double[] inputValue, int amountOfPeriodToCalculate, int movingAverageTerms, double dataSmoothingFactor, double trendSmoothingFactor) 
    {
      double initalSmoothedValue = inputValue[0];
      (List<double>, List<double>) auxValues = 
        CalculteSmoothedAndTrendValues(inputValue.ToList(), dataSmoothingFactor, trendSmoothingFactor, new List<double>(), new List<double>());
      //Calculate forecast for periods which already have real values
      List<double> existantForecastedValues =
        CalculateForecast(auxValues.Item1, auxValues.Item2, new List<double>());
      //Call function to calculate the 'n' next periods
      return Calculate(
        inputValue.Last(), 0, amountOfPeriodToCalculate, new List<double>(), auxValues.Item1.Last(), dataSmoothingFactor, trendSmoothingFactor, auxValues.Item2.Last()).ToArray<double>();
    }
    private static List<double> Calculate(
      double lastRealValue, int index, int amountOfPeriodToCalculate, List<double> outputValue, double smoothedValue, double dataSmoothingValue, double trendSmoothingFactor, double trendValue)
    {
      if (0 == amountOfPeriodToCalculate) return outputValue;
      else
      {
        //Calculate new smoothed value
        double newSmoothedValue = SmoothedValue(dataSmoothingValue, lastRealValue, smoothedValue, trendValue);
        //Calculate new trend value
        double newTrendValue = TrendValue(trendSmoothingFactor, newSmoothedValue, outputValue.First(), trendValue);
        //Add new forecasted value
        outputValue.Add(Forecast(newSmoothedValue, newTrendValue, index));
        return Calculate(lastRealValue, index++, amountOfPeriodToCalculate--, outputValue, newSmoothedValue, dataSmoothingValue, trendSmoothingFactor, newTrendValue);
      }
    }
    private static double SmoothedValue(double dataSmoothingFactor, double realValue, double previousSmoothedValue, double previousTrendValue)
    {
      return dataSmoothingFactor * realValue + (1 - dataSmoothingFactor) * (previousSmoothedValue + previousTrendValue);
    } 
    private static double TrendValue(double trendSmoothingFactor, double smoothedValue, double previousSmoothedValue, double previousTrendValue)
    {
      return trendSmoothingFactor * (smoothedValue - previousSmoothedValue) + (1 - trendSmoothingFactor) * previousTrendValue;
    }
    private static double Forecast(double smoothedValue, double trendValue, int index)
    {
      return smoothedValue + index * trendValue;
    }
    //Starting trend is 1
    private static (List<double>, List<double>) CalculteSmoothedAndTrendValues(
      List<double> realValues, double dataSmoothingFactor, double trendSmoothingFactor, List<double> smoothedValues, List<double> trendValues, bool firstRunFlag = true)
    {
      if (0 == realValues.Count) return (smoothedValues, trendValues);
      //First trend value is always zero
      //First smoothed value is always the same as the real value
      if (firstRunFlag)
      {
        smoothedValues.Add(SmoothedValue(dataSmoothingFactor, realValues.First(), realValues.First(), 1));
        trendValues.Add(0.0);
      }
      else
      {
        smoothedValues.Add(SmoothedValue(dataSmoothingFactor, realValues.First(), smoothedValues.Last(), trendValues.Last()));
        trendValues.Add(TrendValue(trendSmoothingFactor, smoothedValues.Last(), smoothedValues.ElementAt(smoothedValues.Count - 2), trendValues.Last()));
      }
      realValues.RemoveAt(0);
      return CalculteSmoothedAndTrendValues(realValues, dataSmoothingFactor, trendSmoothingFactor, smoothedValues, trendValues, false);
    }
    private static List<double> CalculateForecast(List<double> smoothedValues, List<double> trendValues, List<double> output)
    {
      if(0 == smoothedValues.Count)
      {
        return output;
      }
      else
      {
        output.Add(Forecast(smoothedValues.First(), trendValues.First(), 1));
        smoothedValues.RemoveAt(0);
        trendValues.RemoveAt(0);
      }
      return CalculateForecast(smoothedValues, trendValues, output);
    }
  }
}
