namespace Simulador
{
  class Server
  {
    public Server()
    {
      free = true;
      numberOfCustomersServed = 0;
    }
    public bool free { get; set; }
    public int numberOfCustomersServed { get; set; }
    public double activeTime { get; set; }
    public void AddCustomerServed() => numberOfCustomersServed += 1;
    public void AddActiveTime(double time) => activeTime += time;
  }
}
