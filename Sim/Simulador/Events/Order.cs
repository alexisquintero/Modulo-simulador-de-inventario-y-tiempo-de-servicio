
using Simulador.Generators;

namespace Simulador.Events
{
  class Order : Event {
    public Order()
    {
      Ammount = NormalDIstribution.OrderSize(Inventory.orderSizeMean, Inventory.orderSizeStdDev);
      Time    = NormalDIstribution.TimeBetweenOrder(Inventory.timeBetweenOrdersMean, Inventory.timeBetweenOrdersStdDev);
    }
    public double Ammount { get; }
    public Order GenerateNext() { return new Order(); }
  }
}
