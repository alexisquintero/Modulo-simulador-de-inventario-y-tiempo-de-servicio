using System;

namespace Utils.Exceptions
{
  public class NegativePeriodsToCalculate : Exception
  {
    public static readonly string eMessage = "Negative amount of future periods to calculate not allowed";
    public NegativePeriodsToCalculate() : base(eMessage) { }
    public NegativePeriodsToCalculate(Exception inner) : base(eMessage, inner) { }
    private NegativePeriodsToCalculate(string message, Exception inner) : base(message, inner) { }
  }
}
