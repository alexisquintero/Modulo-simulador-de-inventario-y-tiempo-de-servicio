using Simulador.Utils;
using Simulador.Events;
using static Simulador.Utils.Enumerators;
using System.Linq;

namespace Simulador
{
  class Queue : SimBase
  {
    private Server[] servers;
    //TODO: add different queue modes
    private LinkedList<Arrival> queue;
    private int numberOfClientsOnQueue = 0;
    private int numberOfClientsServed = 0;
    private void Simulation(
      double timeStartSimulation, 
      double timeEndSimulation, 
      int numberOfServers,
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
      int numberOfServers,
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
      servers = new Server[numberOfServers].Select(s => new Server()).ToArray();
    }
    private void Statistics()
    {
      switch (eventList[(int)nextEvent])
      {
        case Arrival a:
          //Get free servers
          Server[] availableServers = servers.Where(s => s.IsFree()).ToArray();
          //If there's no server free add client to queue
          if (availableServers.Length == 0)
          {
            queue.Add(a);
            numberOfClientsOnQueue += 1;
          }
          //If there's at least one server free assign to client
          else
          {
            //Add arrival event to server to change the state to busy 
            availableServers[0].arrival = a;
            //Add 1 to the number of clients attended
            numberOfClientsServed += 1;
            //Generate departure for current client
            if (eventList[(int)EventEnum.Departure] is Departure d)
            {
              eventList[(int)EventEnum.Departure] = d.GenerateEvent();
            }
          }
          break;
        case Departure d:
          break;
      }
    }
  }
}
