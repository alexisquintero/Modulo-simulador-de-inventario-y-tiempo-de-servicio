using Simulador.Events;
using Simulador.Utils;
using static Simulador.Utils.Enumerators;

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
      double orderSizeMean,
      double orderSizeStdDev,
      double timeBetweenOrdersMean,
      double timeBetweenOrderStdDev)
    {
      Initialization();
      while (clock < endTime)
      {
        Statistics();
        GenerateEvent(
          orderSizeMean, 
          orderSizeStdDev, 
          timeBetweenOrdersMean, 
          timeBetweenOrderStdDev);
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
    private static void GenerateEvent(
      double orderSizeMean, 
      double orderSizeStdDev, 
      double timeBetweenOrdersMean, 
      double timeBetweenOrderStdDev)
    {
      //Add event to every event list
      events.Add(eventList[(int)nextEvent]);
      //Generate new event
      Order order = new Order(
        orderSizeMean,
        orderSizeStdDev,
        timeBetweenOrdersMean,
        timeBetweenOrderStdDev);
      //Add event to event list
      eventList[(int)nextEvent] = order;
      //Advance clock to next event
      clock += order.Time;
    }
  }
}