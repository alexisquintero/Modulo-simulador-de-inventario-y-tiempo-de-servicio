using System;

namespace Utils.Exceptions
{
  public class EmptyParameterArray : Exception
  {
    private static readonly string eMessage =
      "Empty parameter array not allowed";
    public EmptyParameterArray()
      : base(eMessage)
    {
    }
    public EmptyParameterArray(Exception inner)
      : base(eMessage, inner)
    {
    }
    private EmptyParameterArray(string message, Exception inner)
      : base(message, inner)
    {
    }
  }
}
