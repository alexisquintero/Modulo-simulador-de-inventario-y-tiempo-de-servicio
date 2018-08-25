
using Simulador.Generators;

namespace Simulador.Events
{
  class Order
  {
    public Order(
      double osMean     = 0, 
      double osStdDev   = 0, 
      double tboMean    = 0, 
      double tboStdDev  = 0)
    {
      Ammount = NormalDIstribution.OrderSize(osMean, osStdDev);
      Time    = NormalDIstribution.TimeBetweenOrder(tboMean, tboStdDev);
    }
    public double Time { get; }
    public double Ammount { get; }
  }
}
