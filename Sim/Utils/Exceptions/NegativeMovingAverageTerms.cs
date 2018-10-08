using System;

namespace Utils.Exceptions
{
  public class NegativeMovingAverageTerms : Exception
  {
    public static readonly string eMessage =
      "Negative amount of moving average terms not allowed";
    public NegativeMovingAverageTerms()
      : base(eMessage)
    {
    }
    public NegativeMovingAverageTerms(Exception inner)
      : base(eMessage, inner)
    {
    }
    private NegativeMovingAverageTerms(string message, Exception inner)
      : base(message, inner)
    {
    }
  }
}
