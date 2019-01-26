using Simulador.Generators;

namespace Simulador.Events
{
  class Departure : Event
  {
    private Departure()
    {
      Init();
    }
    public Departure( double timeMean = 0, double timeStdDev = 0)
    {
      TimeMean   = timeMean;
      TimeStdDev = timeStdDev;
      Init();
    }
    private void Init()
    {
      if (firstEvent)
      {
        Time = double.MaxValue;
        firstEvent = false;
      }
      else
      {
        Time = NormalDIstribution.DepartureTime(TimeMean, TimeStdDev);
      }
    }
    private bool firstEvent = true;
    //Points to the server this departure affects
    public int ServerIndex { get; set; }
    private static double TimeMean;
    private static double TimeStdDev;
    public Departure GenerateEvent()
    {
      return new Departure();
    }
    public Departure GenerateInifiniteTimeEvent()
    {
      firstEvent = true;
      return new Departure();
    }
  }
}
