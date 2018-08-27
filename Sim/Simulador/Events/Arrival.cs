using Simulador.Generators;

namespace Simulador.Events
{
  class Arrival : Event
  {
    public Arrival(
      double timeMean = 0,
      double timeStdDev = 0)
    {
      this.timeMean   = timeMean;
      this.timeStdDev = timeStdDev;
      Time = NormalDIstribution.ArrivalTime(timeMean, timeStdDev);
    }
    private double timeMean;
    private double timeStdDev;
    public double Time { get; }
    public Arrival GenerateNext()
    {
      return new Arrival(timeMean, timeStdDev);
    }
  }
}
