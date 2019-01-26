using System;

namespace Utils.Exceptions
{
  public class SimulationNotInitialized : Exception
  {
    public static readonly string eMessage = "Simulation has not yet been initialized";
    public SimulationNotInitialized() : base(eMessage) { }
    public SimulationNotInitialized(Exception inner) : base(eMessage, inner) { }
    private SimulationNotInitialized(string message, Exception inner) : base(message, inner) { }
  }
}
