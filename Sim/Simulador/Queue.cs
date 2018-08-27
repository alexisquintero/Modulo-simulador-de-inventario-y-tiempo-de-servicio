using Simulador.Utils;
using Simulador.Events;

namespace Simulador
{
  class Queue : SimBase
  {
    private void Simulation(
      double arrivalMean, 
      double arrivalStdDev, 
      double departureMean, 
      double departureStdDev)
    {
      Initialization();
    }
    private void Initialization(
      double startOfSimulationTime, 
      double endOfSimulationTime,
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
      nextEvent = Enumerators.EventEnum.Arrival;
      //Generate first event
      eventList[(int)nextEvent] = (new Arrival(arrivalTimeMean, arrivalTimeStdDev));
    }
  }
}
