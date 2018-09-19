using System;

namespace Utils.Exceptions
{
  public class ZeroInputArray : Exception
  {
    public static readonly string eMessage =
      "Zero on imput array not allowed";
    public ZeroInputArray()
      : base(eMessage)
    {
    }
    public ZeroInputArray(Exception inner)
      : base(eMessage, inner)
    {
    }
    private ZeroInputArray(string message, Exception inner)
      : base(message, inner)
    {
    }
  }
}
