using Simulador.Events;
using Simulador.Utils;
using System;
using System.Collections.Generic;
using Utils.Exceptions;

namespace Simulador
{
  class Inventory : SimBase
  {
    //Determine items to simulate
    //Determine order frequency probability distribution
    //Determine order size probability distribution

    private static double inventory = 0;        //Inventory value at start of simulation
    //TODO: Manage inventory refill, different modes, etc.
    private static double totalDemand = 0;      //Total demanded ammount
    private static double satisfiedDemand = 0;  //Satisfied demand
    private static double missedDemand = 0;     //Not satisfied demand

    private static double initialInventory;
    public static double orderSizeMean;
    public static double orderSizeStdDev;
    public static double timeBetweenOrdersMean;
    public static double timeBetweenOrdersStdDev;

    //Statistics
    private static int totalNumberOfOrders = 0;
    private static int numberOfOrdersNotEnoughStock = 0;
    private void Simulation()
    {
      if (-1 == clock) throw new SimulationNotInitialized();
      while (clock < endTime)
      {
        Statistics();
        GenerateEvent();
      }
    }
    public static void Initialization(double startOfSimulationTime, double endOfSimulationTime,
      double pInitialInventory, double pInventory, List<(DateTime, double)> rawData)
    {
      clock = startOfSimulationTime;
      endTime = endOfSimulationTime;
      initialInventory = pInitialInventory;
      CalculateMeansAndStdDevs(rawData);
      //Generate first event
      eventList[(int)nextEvent] = (new Order());
      //Advance clock of simulation to first event
      if (eventList[(int)nextEvent] is Order o)
      {
        clock += o.Time;
      }
    }
    private static void CalculateMeansAndStdDevs(List<(DateTime, double)> rawData)
    {
      //orderSizeMean = osMean;
      //orderSizeStdDev = osStdDev;
      //timeBetweenOrdersMean = tboMean;
      //timeBetweenOrdersStdDev = tboStdDev;
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