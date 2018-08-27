
using Simulador.Generators;

namespace Simulador.Events
{
  class Order : Event {
    public Order(
      double osMean     = 0, 
      double osStdDev   = 0, 
      double tboMean    = 0, 
      double tboStdDev  = 0)
    {
      this.osMean     = osMean;
      this.osStdDev   = osStdDev;
      this.tboMean    = tboMean;
      this.tboStdDev  = tboStdDev;
      Ammount = NormalDIstribution.OrderSize(osMean, osStdDev);
      Time    = NormalDIstribution.TimeBetweenOrder(tboMean, tboStdDev);
    }
    private double osMean     = 0;
    private double osStdDev   = 0;
    private double tboMean    = 0;
    private double tboStdDev  = 0;
    public double Time { get; }
    public double Ammount { get; }
    public Order GenerateNext()
    {
      return new Order(osMean, osStdDev, tboMean, tboStdDev);
    }
  }
}
