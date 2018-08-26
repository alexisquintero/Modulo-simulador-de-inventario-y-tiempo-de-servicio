using Simulador.Events;
using System;
using static Simulador.Utils.Enumerators;

namespace Simulador.Utils
{
  class SimBase
  {
    protected static double clock = 0;            //Simulation time
    protected static Event[] eventList =
      new Event[Enum.GetNames(typeof(EventEnum)).Length]; //Event list
    protected static EventEnum nextEvent; //Points to the type of next event
    protected static LinkedList<Event> events = 
      new LinkedList<Event>();                    //Every event list
    protected static double endTime;              //Simulation end time  
  }
}
