using Simulador.Events;

namespace Simulador
{
  class Server
  {
    public Server()
    {
      arrival = null;
      numberOfCustomersServed = 0;
    }
    public Arrival arrival { get; set; }
    public int numberOfCustomersServed { get; set; }
    public double activeTime { get; set; }
    public void AddCustomerServed() => numberOfCustomersServed += 1;
    public void AddActiveTime(double time) => activeTime += time;
    public bool IsFree() => arrival == null;
  }
}
