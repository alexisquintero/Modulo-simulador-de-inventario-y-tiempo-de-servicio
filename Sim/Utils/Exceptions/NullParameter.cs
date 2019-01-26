using System;

namespace Utils.Exceptions
{
  public class NullParameter : Exception
  {
    public static readonly string eMessage = "Null parameter not allowed";
    public NullParameter() : base(eMessage) { }
    public NullParameter(Exception inner) : base(eMessage, inner) { }
    private NullParameter(string message, Exception inner) : base(message, inner) { }
  }
}
