using Simulador.Utils;
using Simulador.Events;
using static Simulador.Utils.Enumerators;
using System.Linq;
using System.Collections.Generic;

namespace Simulador
{
  class Queue : SimBase
  {
    private Server[] servers;
    //TODO: add different queue modes
    private List<Arrival> queue;
    private int numberOfClientsOnQueue = 0;
    private int numberOfClientsServed = 0;
    private double queueTime = 0;
    private void Simulation( double timeStartSimulation, double timeEndSimulation, int numberOfServers,
      double arrivalMean, double arrivalStdDev, double departureMean, double departureStdDev)
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
        ClockEventUpdate();
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
        new Arrival(arrivalTimeMean, arrivalTimeStdDev, clock);
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
    private static void GatherData()
    {
      //Do Something
      //Simulation();
    }
    public static void StartSimulation(
      double pEndTime = 0)
    {
      endTime = pEndTime;
      GatherData();
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
            availableServers[0].Arrival = a;
            //Add 1 to the number of clients attended
            numberOfClientsServed += 1;
            //Generate departure for current client
            if (eventList[(int)EventEnum.Departure] is Departure d)
            {
              eventList[(int)EventEnum.Departure] = d.GenerateEvent();
            }
          }
          //Advance clock to next event and set type of next event
          break;
        case Departure d:
          //Check queue
          if (numberOfClientsOnQueue == 0)
          {
            //Remove arrival event from server to set status to free
            servers[d.ServerIndex].Arrival = null;
            //Generate departure with at infinite time
            eventList[(int)EventEnum.Departure] = d.GenerateInifiniteTimeEvent();
          }
          else
          {
            //Calculate wait time
            queueTime += clock - servers[d.ServerIndex].Arrival.TimeOfArrival;
            //Update number of clients served
            numberOfClientsServed += 1;
            //Assign client on queue to server 
            servers[d.ServerIndex].Arrival = queue.First();
            //Remove a client from the queue
            numberOfClientsOnQueue -= 1;
            queue = queue.Skip(1).ToList();
            //Generate next departure time
            servers[d.ServerIndex].Departure = d.GenerateEvent();
            //Update next event departure
            Server closestDepartureServer = servers.OrderByDescending(s =>
                (s.Arrival.TimeOfArrival + s.Departure.Time) - clock).First();
            eventList[(int)EventEnum.Arrival] = closestDepartureServer.Arrival;
          }
          break;
      }
      ClockEventUpdate();
    }
    private void ClockEventUpdate()
    {
      //Get next event
      Event proxEvent = eventList.OrderByDescending(e => e.Time).First<Event>();
      //Move clock forwards
      clock += proxEvent.Time;
      //Set next event type
      switch(proxEvent)
      {
        case Arrival a:
          nextEvent = EventEnum.Arrival;
          break;
        case Departure d:
          nextEvent = EventEnum.Departure;
          break;
      }
    }
  }
}
