using MathNet.Numerics.Distributions;
using MathNet.Numerics.Random;

namespace Simulador.Generators
{
  class NormalDIstribution
  {
    double orderSize(double mean, double stddev)
    {
      Normal normal = new Normal(mean, stddev);
      return normal.Sample();
    }
    double timeBetweenOrder(double mean, double stddev)
    {
      Normal normal = new Normal(mean, stddev);
      return normal.Sample();
    }
  }
}
