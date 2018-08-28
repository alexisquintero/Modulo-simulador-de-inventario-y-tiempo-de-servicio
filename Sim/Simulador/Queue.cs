using Simulador.Utils;
using Simulador.Events;
using static Simulador.Utils.Enumerators;

namespace Simulador
{
  class Queue : SimBase
  {
    private Server[] servers;
    private void Simulation(
      double timeStartSimulation, 
      double timeEndSimulation, 
      double numberOfServers,
      double arrivalMean, 
      double arrivalStdDev, 
      double departureMean, 
      double departureStdDev)
    {
      Initialization(
        timeStartSimulation, 
        timeEndSimulation, 
        numberOfServers, 
        arrivalMean, 
        arrivalStdDev, 
        departureMean, 
        departureStdDev);
      while (clock < endTime)
      {
        Statistics();
        GenerateEvent();
      }
    }
    private void Initialization(
      double startOfSimulationTime, 
      double endOfSimulationTime,
      double numberOfServers,
      double arrivalTimeMean, 
      double arrivalTimeStdDev, 
      double departureTimeMean, 
      double departureTimeStdDev)
    {
      //Set the start of simultion time
      clock = startOfSimulationTime;
      //Set the end of simulation time
      endTime = endOfSimulationTime;
      //Set next arrival as next event
      nextEvent = EventEnum.Arrival;
      //Generate first events
      eventList[(int)nextEvent] = 
        new Arrival(arrivalTimeMean, arrivalTimeStdDev);
      eventList[(int)EventEnum.Departure] =
        new Departure(departureTimeMean, departureTimeStdDev);
      //Advance clock of simulation to first event
      if (eventList[(int)nextEvent] is Arrival a)
      {
        clock += a.Time;
      }
      //Create servers
      for (int i = 0; i < numberOfServers; i++)
      {
        servers[i] = new Server();
      }
    }
  }
}
