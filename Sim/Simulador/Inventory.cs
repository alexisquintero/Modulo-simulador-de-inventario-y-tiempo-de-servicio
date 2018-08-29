using Simulador.Events;
using Simulador.Utils;

namespace Simulador
{
  class Inventory : SimBase
  {
    //Determine items to simulate
    //Determine order frequency probability distribution
    //Determine order size probability distribution

    private static double inventory;  //Inventory value at start of simulation
    //TODO: Manage inventory refill, different modes, etc.
    private static double totalDemand = 0;      //Total demanded ammount
    private static double satisfiedDemand = 0;  //Satisfied demand
    private static double missedDemand = 0;     //Not satisfied demand

    //Statistics
    private static int totalNumberOfOrders = 0;
    private static int numberOfOrdersNotEnoughStock = 0;
    private void Simulation(
      double timeStartSimulation, 
      double timeEndSimulation, 
      double initialInventory,
      double orderSizeMean,
      double orderSizeStdDev,
      double timeBetweenOrdersMean,
      double timeBetweenOrdersStdDev)
    {
      Initialization(
        timeStartSimulation, 
        timeEndSimulation, 
        initialInventory,
        orderSizeMean, 
        orderSizeStdDev, 
        timeBetweenOrdersMean, 
        timeBetweenOrdersStdDev);
      while (clock < endTime)
      {
        Statistics();
        GenerateEvent();
      }
    }
    private void GatherData()
    {
      //Do something
      Simulation();
    }
    public static void StartSimulation(
      double pInventory = 0, 
      double pEndTime = 0)
    {
      inventory = pInventory;
      endTime = pEndTime;
    }
    private static void Initialization(
      double startOfSimulationTime,
      double endOfSimulationTime,
      double initialInventory,
      double osMean, 
      double osStdDev, 
      double tboMean, 
      double tboStdDev)
    {
      //Set the end of simulation time
      endTime = endOfSimulationTime;
      //Set the start of simultion time
      clock = startOfSimulationTime;
      //Generate first event
      eventList[(int)nextEvent] = (new Order(osMean, osStdDev, tboMean, tboStdDev));
      //Advance clock of simulation to first event
      if (eventList[(int)nextEvent] is Order o)
      {
        clock += o.Time;
      }
    }
    private static void Statistics()
    {
      totalNumberOfOrders += 1;
      if (eventList[(int)nextEvent] is Order o)
      {
        if (inventory < o.Ammount)
        {
          inventory = 0;
          numberOfOrdersNotEnoughStock += 1;
          missedDemand += inventory == 0 ?
            o.Ammount : inventory - o.Ammount;
        }
        else
        {
          satisfiedDemand += o.Ammount;
        }
        totalDemand += o.Ammount;
      }
    }
    private static void GenerateEvent()
    {
      //Add event to every event list
      events.Add(eventList[(int)nextEvent]);
      if (eventList[(int)nextEvent] is Order o)
      {
        //Generate new event
        Order order = o.GenerateNext();
        //Add new event to event list
        eventList[(int)nextEvent] = o.GenerateNext();
        //Advance clock to next event
        clock += order.Time;
      }
    }
  }
}