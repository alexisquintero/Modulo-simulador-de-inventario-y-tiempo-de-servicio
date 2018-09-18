using System;

namespace Utils.Exceptions
{
  public class DifferentSizeArrays : Exception
  {
    private static readonly string eMessage =
      "Different sized parameter arrays not allowed";
    public DifferentSizeArrays()
      : base(eMessage)
    {
    }
    public DifferentSizeArrays(Exception inner)
      : base(eMessage, inner)
    {
    }
    private DifferentSizeArrays(string message, Exception inner)
      : base(message, inner)
    {
    }
  }
}
