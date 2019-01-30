using Utils.Exceptions;
using MathNet.Numerics.Distributions;
using Utils;

namespace Simulador.Generators
{
  public abstract class Distribution
  {
    protected static IDistribution distributionOrderSize;
    protected static IDistribution distributionTimeBetweenOrder;
    protected static IDistribution distributionArrivalTime;
    protected static IDistribution distributionDepartureTime;
    public static double OrderSize() { throw new DistributionMethod(); }

    public static double TimeBetweenOrder() { throw new DistributionMethod(); }

    public static double ArrivalTime() { throw new DistributionMethod(); }

    public static double DepartureTime() { throw new DistributionMethod(); }
  }
  public class NormalDistribution: Distribution
  {
    public static void Initialize(double mean, double stddev, Generator g)
    {
      switch (g)
      {
        case Generator.OrderSize: distributionOrderSize = new Normal(mean, stddev); break;
        case Generator.TimeBetweenOrders: distributionTimeBetweenOrder = new Normal(mean, stddev); break;
        default: break;
      }
    }
    new public static double OrderSize() { return ((Normal)distributionOrderSize).Sample(); }
    new public static double TimeBetweenOrder() { return ((Normal)distributionTimeBetweenOrder).Sample(); }
    new public static double ArrivalTime() { return ((Normal)distributionArrivalTime).Sample(); }
    new public static double DepartureTime() { return ((Normal)distributionDepartureTime).Sample(); }
  }
  public class PoissonDistribution: Distribution
  {
    public static void Initialize(double lambda, Generator g)
    {
      switch (g)
      {
        case Generator.OrderSize: distributionOrderSize = new Poisson(lambda); break;
        case Generator.TimeBetweenOrders: distributionTimeBetweenOrder = new Poisson(lambda); break;
        default: break;
      }
    }
    new public static double OrderSize() { return ((Poisson)distributionOrderSize).Sample(); }
    new public static double TimeBetweenOrder() { return ((Poisson)distributionTimeBetweenOrder).Sample(); }
    new public static double ArrivalTime() { return ((Poisson)distributionArrivalTime).Sample(); }
    new public static double DepartureTime() { return ((Poisson)distributionDepartureTime).Sample(); }
  }
  public class ExponentialDistribution: Distribution
  {
    public static void Initialize(double rate, Generator g)
    {
      switch (g)
      {
        case Generator.OrderSize: distributionOrderSize = new Exponential(rate); break;
        case Generator.TimeBetweenOrders: distributionTimeBetweenOrder = new Exponential(rate); break;
        default: break;
      }
    }
    new public static double OrderSize() { return ((Exponential)distributionOrderSize).Sample(); }
    new public static double TimeBetweenOrder() { return ((Exponential)distributionTimeBetweenOrder).Sample(); }
    new public static double ArrivalTime() { return ((Exponential)distributionArrivalTime).Sample(); }
    new public static double DepartureTime() { return ((Exponential)distributionDepartureTime).Sample(); }
  }
  public class UniformContDistribution: Distribution
  {
    public static void Initialize(double lower, double upper, Generator g)
    {
      switch (g)
      {
        case Generator.OrderSize: distributionOrderSize = new ContinuousUniform(lower, upper); break;
        case Generator.TimeBetweenOrders: distributionTimeBetweenOrder = new ContinuousUniform(lower, upper); break;
        default: break;
      }
    }
    new public static double OrderSize() { return ((ContinuousUniform)distributionOrderSize).Sample(); }
    new public static double TimeBetweenOrder() { return ((ContinuousUniform)distributionTimeBetweenOrder).Sample(); }
    new public static double ArrivalTime() { return ((ContinuousUniform)distributionArrivalTime).Sample(); }
    new public static double DepartureTime() { return ((ContinuousUniform)distributionDepartureTime).Sample(); }
  }
  public class UniformDiscDistribution: Distribution
  {
    public static void Initialize(int lower, int upper, Generator g)
    {
      switch (g)
      {
        case Generator.OrderSize: distributionOrderSize = new DiscreteUniform(lower, upper); break;
        case Generator.TimeBetweenOrders: distributionTimeBetweenOrder = new DiscreteUniform(lower, upper); break;
        default: break;
      }
    }
    new public static double OrderSize() { return ((DiscreteUniform)distributionOrderSize).Sample(); }
    new public static double TimeBetweenOrder() { return ((DiscreteUniform)distributionTimeBetweenOrder).Sample(); }
    new public static double ArrivalTime() { return ((DiscreteUniform)distributionArrivalTime).Sample(); }
    new public static double DepartureTime() { return ((DiscreteUniform)distributionDepartureTime).Sample(); }
  }
}
