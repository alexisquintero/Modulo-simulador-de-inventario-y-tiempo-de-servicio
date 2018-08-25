using MathNet.Numerics.Distributions;

namespace Simulador.Generators
{
  class NormalDIstribution
  {
    public static double OrderSize(double mean, double stddev)
    {
      Normal normal = new Normal(mean, stddev);
      return normal.Sample();
    }
    public static double TimeBetweenOrder(double mean, double stddev)
    {
      Normal normal = new Normal(mean, stddev);
      return normal.Sample();
    }
  }
}
