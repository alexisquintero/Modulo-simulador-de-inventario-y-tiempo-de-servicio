using MathNet.Numerics.Statistics;
using Simulador.Events;
using Simulador.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using Utils.Exceptions;
using static Simulador.Utils.Enumerators;

namespace Simulador
{
  public class Inventory 
  {
    //Determine items to simulate
    //Determine order frequency probability distribution
    //Determine order size probability distribution

    private static double clock = -1;                       //Simulation time
    private static Event[] eventList = new Event[Enum.GetNames(typeof(EventEnum)).Length]; //Event list
    private static EventEnum nextEvent;                     //Points to the type of next event
    private static List<Event> events = new List<Event>();  //Every event list
    private static double endTime;                          //Simulation end time  

    private static double inventory = 0;        //Inventory value at start of simulation
    //TODO: Manage inventory refill, different modes, etc.
    private static double totalDemand = 0;      //Total demanded ammount
    private static double satisfiedDemand = 0;  //Satisfied demand
    private static double missedDemand = 0;     //Not satisfied demand

    public static double orderSizeMean;
    public static double orderSizeStdDev;
    public static double timeBetweenOrdersMean;
    public static double timeBetweenOrdersStdDev;

    private static DateTime firstDateTime;

    //Statistics
    private static int totalNumberOfOrders = 0;
    private static int numberOfOrdersNotEnoughStock = 0;
    public static List<(DateTime, double)> Simulation()
    {
      if (-1 == clock) throw new SimulationNotInitialized();
      while (clock < endTime)
      {
        Statistics();
        GenerateEvent();
      }

      List<(DateTime, double)> returnData = new List<(DateTime, double)>
      {
        (firstDateTime.AddSeconds(events.First().Time), ((Order)events.First()).Ammount)
      };
      for (int i = 1; i < events.Count; i++)
      {
        returnData.Add(
          (returnData.ElementAt(i-1).Item1.AddSeconds(events.ElementAt(i).Time), 
          ((Order)events.ElementAt(i)).Ammount)
        );
      }

      return returnData;
    }
    public static void Initialization(double startOfSimulationTime, double endOfSimulationTime,
      double pInitialInventory, List<(DateTime, double)> rawData)
    {
      clock = startOfSimulationTime;
      endTime = endOfSimulationTime;
      inventory = pInitialInventory;
      firstDateTime = rawData.First().Item1;
      CalculateMeansAndStdDevs(rawData);
      nextEvent = EventEnum.Order;
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
      List<double> amounts = new List<double>();
      List<DateTime> dates = new List<DateTime>();
      foreach ((DateTime, double) rd in rawData) { dates.Add(rd.Item1); amounts.Add(rd.Item2); }

      Tuple<double, double> osMeanStdDev = amounts.ToArray().MeanStandardDeviation();
      orderSizeMean = osMeanStdDev.Item1;
      orderSizeStdDev = osMeanStdDev.Item2;

      //For tbo we use the time between orders in seconds
      //Max int in seconds is approximately 68 years
      List<double> secondsBetweenOrders = new List<double>();
      for (int i = 0; i < dates.Count - 1; i++)
      {
        secondsBetweenOrders.Add(dates.ElementAt(i + 1).Subtract(dates.ElementAt(i)).TotalSeconds);
      }
      Tuple<double, double> tboMeanStdDev = secondsBetweenOrders.ToArray().MeanStandardDeviation();
      timeBetweenOrdersMean = tboMeanStdDev.Item1;
      timeBetweenOrdersStdDev = tboMeanStdDev.Item2;
    }
    private static void Statistics()
    {
      totalNumberOfOrders++;
      if (eventList[(int)nextEvent] is Order o)
      {
        if (inventory < o.Ammount)
        {
          numberOfOrdersNotEnoughStock += 1;
          missedDemand += inventory == 0 ?  o.Ammount : inventory - o.Ammount;
          inventory = 0;
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