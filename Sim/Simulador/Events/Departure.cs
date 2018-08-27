using Simulador.Generators;

namespace Simulador.Events
{
  class Departure : Event
  {
    public Departure(
      double timeMean   = 0,
      double timeStdDev = 0)
    {
      this.timeMean   = timeMean;
      this.timeStdDev = timeStdDev;
      if (firstEvent)
      {
        Time = double.MaxValue;
        firstEvent = false;
      } else
      {
        Time = NormalDIstribution.DepartureTime(timeMean, timeStdDev);
      }
    }
    private bool firstEvent = true;
    private double timeMean;
    private double timeStdDev;
    public double Time { get; }
    public Departure GenerateEvent()
    {
      return new Departure(timeMean, timeStdDev);
    }
  }
}
