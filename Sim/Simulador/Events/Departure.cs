using Simulador.Generators;

namespace Simulador.Events
{
  class Departure : Event
  {
    public Departure(
      double timeMean   = 0,
      double timeStdDev = 0)
    {
      Time = NormalDIstribution.DepartureTime(timeMean, timeStdDev);
    }
    public double Time { get; }
  }
}
