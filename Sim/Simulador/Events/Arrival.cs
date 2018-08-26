using Simulador.Generators;

namespace Simulador.Events
{
  class Arrival : Event
  {
    public Arrival(
      double timeMean = 0,
      double timeStdDev = 0)
    {
      Time = NormalDIstribution.ArrivalTime(timeMean, timeStdDev);
    }
      public double Time { get; }
  }
}
