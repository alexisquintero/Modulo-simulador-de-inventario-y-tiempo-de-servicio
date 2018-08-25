using Simulador.Events;
using System.Collections.Generic;

namespace Simulador
{
  class Inventory
  {
    //Determine items to simulate
    //Determine order frequency probability distribution
    //Determine order size probability distribution

    private static double inventory;            //Actual inventory value
    private static double demand = 0;           //Satisfied demand
    private static double missedDemand = 0;     //Not satisfied demand
    private static double clock = 0;            //Simulation time
    private static double endTime;              //Simulation end time  
    private static List<Order> events = new List<Order>(); //Event list
    private void Simulation(
      double orderSizeMean,
      double orderSizeStdDev,
      double timeBetweenOrdersMean,
      double timeBetweenOrderStdDev)
    {
      Initialization();
      while (clock < endTime)
      { 
        //Generate new event
        Order order = new Order(
          orderSizeMean, 
          orderSizeStdDev, 
          timeBetweenOrdersMean, 
          timeBetweenOrderStdDev);
        //Advance clock to next event
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
    private static void Initialization()
    {
      //Generate first event
    }
  }
}
