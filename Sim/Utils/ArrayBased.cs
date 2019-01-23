using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Utils.Exceptions;

namespace Utils
{
  public class ArrayBased
  {
    public static (double[], double[]) Split(double[] full, int amountOfPeriodsToCalculate)
    {
      int length = full.Length;
      double[] future = full.Skip(full.Length - amountOfPeriodsToCalculate).ToArray();
      double[] present = full.Take(full.Length - future.Length).ToArray();
      return (present, future);
    }
    public static (double[], double[]) Split(List<double> full, int amountOfPeriodsToCalculate)
    {
      return Split(full.ToArray(), amountOfPeriodsToCalculate);
    }
    public static double[] Join((double[], double[]) split)
    {
      return split.Item1.Concat(split.Item2).ToArray();
    }
    public static double[] ShortenRealValueArray(double[] real, double[] forecast)
    {
      int r = real.Length;
      int f = forecast.Length;
      //Same length, do nothing
      if (r == f) return real;
      //Forecast longer than real, throw exception
      else if (f > r) throw new DifferentSizeArrays();
      //Real longer than forecast, drop real starting values to match length
      else return real.Skip(r - f).ToArray();
    }
  }
}
