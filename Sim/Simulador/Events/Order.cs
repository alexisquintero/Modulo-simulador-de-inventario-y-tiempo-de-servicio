using Simulador.Generators;
using System;
using Utils;

namespace Simulador.Events
{
  public class Order : Event
  {
    private Distributions ammountDistribution;
    private Distributions timeDistribution;
    public Order(Distributions ad, Distributions td)
    {
      ammountDistribution = ad;
      timeDistribution = td;

      Ammount = NonNegativeValue(new Func<double>(() =>
      {
        switch (ammountDistribution)
        {
          case Distributions.Normal: return NormalDistribution.OrderSize();
          case Distributions.Poisson: return PoissonDistribution.OrderSize();
          case Distributions.Exponential: return ExponentialDistribution.OrderSize();
          case Distributions.UniformCont: return UniformContDistribution.OrderSize();
          case Distributions.UniformDisc: return UniformDiscDistribution.OrderSize();
          default: return -1;
        }
      })());

      Time = NonNegativeValue(new Func<double>(() =>
      {
        switch (timeDistribution)
        {
          case Distributions.Normal: return NormalDistribution.TimeBetweenOrder();
          case Distributions.Poisson: return PoissonDistribution.TimeBetweenOrder();
          case Distributions.Exponential: return ExponentialDistribution.TimeBetweenOrder();
          case Distributions.UniformCont: return UniformContDistribution.TimeBetweenOrder();
          case Distributions.UniformDisc: return UniformDiscDistribution.TimeBetweenOrder();
          default: return -1;
        }
      })());
    }
    private double NonNegativeValue(double d) { return d = d < 0 ? 0 : d; }
    public double Ammount { get; }
    public Order GenerateNext() { return new Order(ammountDistribution, timeDistribution); }
  }
}
