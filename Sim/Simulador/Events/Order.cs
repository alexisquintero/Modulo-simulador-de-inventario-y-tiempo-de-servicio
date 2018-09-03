
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
      OsMean     = osMean;
      OsStdDev   = osStdDev;
      TboMean    = tboMean;
      TboStdDev  = tboStdDev;
      Ammount = NormalDIstribution.OrderSize(osMean, osStdDev);
      Time    = NormalDIstribution.TimeBetweenOrder(tboMean, tboStdDev);
    }
    private static double OsMean     = 0;
    private static double OsStdDev   = 0;
    private static double TboMean    = 0;
    private static double TboStdDev  = 0;
    public double Ammount { get; }
    public Order GenerateNext()
    {
      return new Order(OsMean, OsStdDev, TboMean, TboStdDev);
    }
  }
}
