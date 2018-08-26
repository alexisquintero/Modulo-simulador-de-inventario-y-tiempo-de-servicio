using Simulador.Events;
using Simulador.Utils;

namespace Simulador
{
  class Inventory
  {
    //Determine items to simulate
    //Determine order frequency probability distribution
    //Determine order size probability distribution

    private static double inventory;  //Inventory value at start of simulation
    //TODO: Manage inventory refill, different modes, etc.
    private static double totalDemand = 0;      //Total demanded ammount
    private static double satisfiedDemand = 0;  //Satisfied demand
    private static double missedDemand = 0;     //Not satisfied demand
    private static double clock = 0;            //Simulation time
    private static double endTime;              //Simulation end time  
    private static LinkedList<Order> eventList = 
      new LinkedList<Order>();                  //Event list
    private static LinkedList<Order> events = 
      new LinkedList<Order>();                  //Every event list

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
      double endOfSimulationTime,
      double startOfSimulationTime,
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
      eventList.Add(new Order(osMean, osStdDev, tboMean, tboStdDev));
      //Advance clock of simulation to first event
      clock += eventList.Head().Time;
    }
    private static void Statistics()
    {
      totalNumberOfOrders += 1;
      if (inventory < eventList.Head().Ammount)
      {
        inventory = 0;
        numberOfOrdersNotEnoughStock += 1;
        missedDemand += inventory == 0 ? 
          eventList.Head().Ammount : inventory - eventList.Head().Ammount;
      }
      else
      {
        satisfiedDemand += eventList.Head().Ammount;
      }
      totalDemand += eventList.Head().Ammount;
    }
    private static void GenerateEvent(
      double orderSizeMean, 
      double orderSizeStdDev, 
      double timeBetweenOrdersMean, 
      double timeBetweenOrderStdDev)
    {
      //Add event to every event list
      events.Add(eventList.Head());
      //Remove event from event list
      eventList = eventList.Tail();
      //Generate new event
      Order order = new Order(
        orderSizeMean,
        orderSizeStdDev,
        timeBetweenOrdersMean,
        timeBetweenOrderStdDev);
      //Add event to event list
      eventList.Add(order);
      //Advance clock to next event
      clock += eventList.Head().Time;
    }
  }
}