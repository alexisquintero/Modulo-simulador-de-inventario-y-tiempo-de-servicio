using System;

namespace Utils.Exceptions
{
  public class DistributionMethod : Exception
  {
    public static readonly string eMessage = "Distribution unhidden method call";
    public DistributionMethod() : base(eMessage) { }
    public DistributionMethod(Exception inner) : base(eMessage, inner) { }
    private DistributionMethod(string message, Exception inner) : base(message, inner) { }
  }
}
