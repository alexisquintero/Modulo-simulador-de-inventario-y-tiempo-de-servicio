using System;

namespace Utils.Exceptions
{
  public class BaseDataMethod : Exception
  {
    public static readonly string eMessage =
      "Base data unhidden method call";
    public BaseDataMethod()
      : base(eMessage)
    {
    }
    public BaseDataMethod(Exception inner)
      : base(eMessage, inner)
    {
    }
    private BaseDataMethod(string message, Exception inner)
      : base(message, inner)
    {
    }
  }
}
