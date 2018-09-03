using Simulador.Events;
using Simulador.Utils;

namespace Simulador
{
  class Server
  {
    public Server()
    {
      Arrival = null; 
      NumberOfCustomersServed = 0;
      ServerIndex = serverIndex;
      serverIndex++;
    }
    public Arrival Arrival { get; set; }
    public Departure Departure { get; set; }
    public int NumberOfCustomersServed { get; set; }
    public double ActiveTime { get; set; }
    public void AddCustomerServed() => NumberOfCustomersServed += 1;
    public void AddActiveTime(double time) => ActiveTime += time;
    public bool IsFree() => Arrival == null;
    private static int serverIndex = 0;
    public int ServerIndex { get; private set; }
  }
}
