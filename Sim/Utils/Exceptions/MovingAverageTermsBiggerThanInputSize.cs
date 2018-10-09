using System;

namespace Utils.Exceptions
{
  public class MovingAverageTermsBiggerThanInputSize : Exception
  {
    public static readonly string eMessage =
      "Moving Average term bigger than input size not allowed";
    public MovingAverageTermsBiggerThanInputSize()
      : base(eMessage)
    {
    }
    public MovingAverageTermsBiggerThanInputSize(Exception inner)
      : base(eMessage, inner)
    {
    }
    private MovingAverageTermsBiggerThanInputSize(string message, Exception inner)
      : base(message, inner)
    {
    }
  }
}
