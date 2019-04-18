using MathNet.Numerics;
using MathNet.Numerics.Statistics;
using Simulador.Events;
using Simulador.Generators;
using System;
using System.Collections.Generic;
using System.Linq;
using Utils;
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

    private static double inventory = 0;        
    //TODO: Manage inventory refill, different modes, etc.
    private static double totalDemand = 0;      //Total demanded ammount
    private static double satisfiedDemand = 0;  //Satisfied demand
    private static double missedDemand = 0;     //Not satisfied demand

    private static DateTime firstDateTime;
    private static Distributions orderAmmount;
    private static Distributions timeBetweenOrders;
    private static Period period;

    //Statistics
    private static int totalNumberOfOrders = 0;
    private static int numberOfOrdersNotEnoughStock = 0;

    private static double orderCoefficientOfDetermination;
    private static double tboCoefficientOfDetermination;
    private static List<double> secondsBetweenOrders = new List<double>();
    public static InventoryOutput Simulation(double startOfSimulationTime, double endOfSimulationTime,
      double pInitialInventory, List<(DateTime, double)> rawData, Distributions pOrderAmmount, 
      Distributions pTimeBetweenOrders, Period p)
    {
      Initialization(startOfSimulationTime, endOfSimulationTime, pInitialInventory, rawData, pOrderAmmount,
        pTimeBetweenOrders, p);
      if (-1 == clock) throw new SimulationNotInitialized();
      while (clock < endTime)
      {
        Statistics();
        GenerateEvent();
      }

      List<(DateTime, double)> returnData = new List<(DateTime, double)>();
      if (events.Count > 0) {
        returnData.Add((firstDateTime.AddSeconds(events.First().Time), ((Order)events.First()).Ammount));
      };
      for (int i = 1; i < events.Count; i++)
      {
        returnData.Add(
          (returnData.ElementAt(i-1).Item1.AddSeconds(events.ElementAt(i).Time), 
          ((Order)events.ElementAt(i)).Ammount)
        );
      }

      //Orders
      orderCoefficientOfDetermination = -1;

      List<(DateTime, double)> grouppedEvents = new List<(DateTime, double)>();
      switch (period)
      {
        case Period.Diario:
          grouppedEvents = returnData.GroupBy(
            s => new DateTime(s.Item1.Year, s.Item1.Month, s.Item1.Day),
            s => s.Item2,
            (date, doub) => (date, doub.Sum())
          ).OrderBy(g => g.Item1).ToList();
          break;
        case Period.Mensual:
          grouppedEvents = returnData.GroupBy(
            s => new DateTime(s.Item1.Year, s.Item1.Month, 1),
            s => s.Item2,
            (date, doub) => (date, doub.Sum())
          ).OrderBy(g => g.Item1).ToList();
          break;
        case Period.Anual:
          grouppedEvents = returnData.GroupBy(
            s => new DateTime(s.Item1.Year, 1, 1),
            s => s.Item2,
            (date, doub) => (date, doub.Sum())
          ).OrderBy(g => g.Item1).ToList();
          break;
        default: break;
      }

      double[] xdata = rawData.Select(r => r.Item2).ToArray();
      double[] ydata = grouppedEvents.Select(g => g.Item2).ToArray();
      int shortest = xdata.Length < ydata.Length ? xdata.Length : ydata.Length;
      Random rnd = new Random();
      double[] nxdata = xdata.OrderBy(x => rnd.Next()).Take(shortest).ToArray();
      double[] nydata = ydata.OrderBy(x => rnd.Next()).Take(shortest).ToArray();
      //if(events.Count > 1)
      if (nxdata.Count() > 1 && nydata.Count() > 1)
      {
        Tuple<double, double> f = Fit.Line(nxdata, nydata);
        double a = f.Item1; double b = f.Item2;
        orderCoefficientOfDetermination = GoodnessOfFit.RSquared(nxdata.Select(x => a + b * x), nydata);
      }

      //Tbo
      tboCoefficientOfDetermination = -1;
      //double[] xdata2 = secondsBetweenOrders.ToArray();
      //double[] ydata2 = events.Select(e => ((Order)e).Time).OrderBy(x => x).ToArray();
      //shortest = xdata2.Length < ydata2.Length ? xdata2.Length : ydata2.Length;
      //double[] nxdata2 = xdata2.Take(shortest).ToArray();
      //double[] nydata2 = ydata2.Take(shortest).ToArray();
      //Tuple<double, double> f2 = Fit.Line(nxdata2, nydata2);
      //double a2 = f2.Item1; double b2 = f2.Item2;
      //tboCoefficientOfDetermination = GoodnessOfFit.RSquared(nxdata2.Select(x => a2 + b2 * x), nydata2);

      return new InventoryOutput(totalDemand, satisfiedDemand, missedDemand, returnData, rawData, period, orderAmmount,
        timeBetweenOrders, orderCoefficientOfDetermination, tboCoefficientOfDetermination);
    }
    private static void Initialization(double startOfSimulationTime, double endOfSimulationTime,
      double pInitialInventory, List<(DateTime, double)> rawData, Distributions pOrderAmmount, 
      Distributions pTimeBetweenOrders, Period p)
    {
      //Reset variables
      events = new List<Event>();
      totalDemand = satisfiedDemand = missedDemand = 0;
      totalNumberOfOrders = numberOfOrdersNotEnoughStock = 0;

      clock = startOfSimulationTime;
      endTime = endOfSimulationTime;
      inventory = pInitialInventory;
      firstDateTime = rawData.First().Item1;
      orderAmmount = pOrderAmmount;
      timeBetweenOrders = pTimeBetweenOrders;
      period = p;
      CalculateDistributionsParameters(rawData);
      nextEvent = EventEnum.Order;
      //Generate first event
      eventList[(int)nextEvent] = (new Order(orderAmmount, timeBetweenOrders));
      //Advance clock of simulation to first event
      if (eventList[(int)nextEvent] is Order o)
      {
        clock += o.Time;
      }
    }
    private static void CalculateDistributionsParameters(List<(DateTime, double)> rawData)
    {
      List<double> amounts = new List<double>();
      List<DateTime> dates = new List<DateTime>();
      double lowest = double.MaxValue;
      double max = double.MinValue;
      foreach ((DateTime, double) rd in rawData) {
        dates.Add(rd.Item1); amounts.Add(rd.Item2);
        lowest = rd.Item2 < lowest ? rd.Item2 : lowest;
        max = rd.Item2 > max ? rd.Item2 : max;
      }

      Tuple<double, double> osMeanStdDev = amounts.ToArray().MeanStandardDeviation();

      switch (orderAmmount)
      {
        case Distributions.Normal:
          NormalDistribution.Initialize(osMeanStdDev.Item1, osMeanStdDev.Item2, Generator.OrderSize); break;
        case Distributions.Poisson:
          PoissonDistribution.Initialize(osMeanStdDev.Item1, Generator.OrderSize); break;
        case Distributions.Exponential:
          ExponentialDistribution.Initialize(1 / osMeanStdDev.Item1, Generator.OrderSize); break;
        case Distributions.UniformCont:
          UniformContDistribution.Initialize(lowest, max, Generator.OrderSize); break;
        case Distributions.UniformDisc:
          //Truncating gives me floor value already
          UniformDiscDistribution.Initialize((int)Math.Ceiling(lowest), (int)max, Generator.OrderSize); break;
        default: break;
      }

      //For tbo we use the time between orders in seconds
      //Max int in seconds is approximately 68 years
      for (int i = 0; i < dates.Count - 1; i++)
      {
        secondsBetweenOrders.Add(dates.ElementAt(i + 1).Subtract(dates.ElementAt(i)).TotalSeconds);
      }
      Tuple<double, double> tboMeanStdDev = secondsBetweenOrders.ToArray().MeanStandardDeviation();

      switch (timeBetweenOrders)
      {
        case Distributions.Normal:
          NormalDistribution.Initialize(tboMeanStdDev.Item1, tboMeanStdDev.Item2, Generator.TimeBetweenOrders); break;
        case Distributions.Poisson:
          PoissonDistribution.Initialize(tboMeanStdDev.Item1, Generator.TimeBetweenOrders); break;
        case Distributions.Exponential:
          ExponentialDistribution.Initialize(1 / tboMeanStdDev.Item1, Generator.TimeBetweenOrders); break;
        case Distributions.UniformCont:
          UniformContDistribution.Initialize(lowest, max, Generator.TimeBetweenOrders); break;
        case Distributions.UniformDisc:
          //Truncating gives me floor value already
          UniformDiscDistribution.Initialize((int)Math.Ceiling(lowest), (int)max, Generator.TimeBetweenOrders); break;
        default: break;
      }
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