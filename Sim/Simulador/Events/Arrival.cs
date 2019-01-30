using Simulador.Generators;

namespace Simulador.Events
{
  class Arrival : Event
  {
    public Arrival(double timeMean = 0, double timeStdDev = 0, double clockOfSimulation = 0)
    {
      TimeMean   = timeMean;
      TimeStdDev = timeStdDev;
      Time = NormalDistribution.ArrivalTime();
      TimeOfArrival = clockOfSimulation;
    }
    private static double TimeMean;
    private static double TimeStdDev;
    public double TimeOfArrival { get; set; }
    public Arrival GenerateNext(double clockOfSimulation)
    {
      return new Arrival(TimeMean, TimeStdDev, clockOfSimulation);
    }
  }
}
