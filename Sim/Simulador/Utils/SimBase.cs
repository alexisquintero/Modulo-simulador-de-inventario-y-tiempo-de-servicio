using Simulador.Events;
using System;
using System.Collections.Generic;
using static Simulador.Utils.Enumerators;

namespace Simulador.Utils
{
  class SimBase
  {
    protected static double clock = -1;                       //Simulation time
    protected static Event[] eventList = new Event[Enum.GetNames(typeof(EventEnum)).Length]; //Event list
    protected static EventEnum nextEvent;                     //Points to the type of next event
    protected static List<Event> events = new List<Event>();  //Every event list
    protected static double endTime;                          //Simulation end time  
  }
}
