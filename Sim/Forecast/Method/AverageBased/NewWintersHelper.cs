using System.Collections.Generic;
using System.Linq;

namespace Forecast.Method.AverageBased
{
  class NewWintersHelper
  {
    //131
    private static List<double> Y = new List<double>();
    private static List<double> L = new List<double>();
    private static List<double> T = new List<double>();
    private static List<double> S = new List<double>();
    private static List<double> F1 = new List<double>();
    private static List<double> F2 = new List<double>();
    private static double alpha;
    private static double beta;
    private static double gamma;
    public static double Lt(double Yt, double Sts, double Lt1,double Tt1)
    { return alpha * Yt / Sts + (1 - alpha) * (Lt1 + Tt1); }
    public static double Tt(double Lt, double Lt1, double Tt1)
    { return beta * (Lt - Lt1) + (1 - beta) * Tt1; }
    public static double St(double Yt, double Lt, double Sts)
    { return gamma * Yt / Lt + (1 - gamma) * Sts; }
    public static double Ytp(double Lt, int p, double Tt, double Stsp)
    { return (Lt + p * Tt) * Stsp; }
    private static double GetSts(int t, int s)
    {
      int index = (t - s) - 1;
      return index < 0 ? 1.0 : S.ElementAt(index);
    }
    public static void Init(double[] pY, int s, double palpha, double pbeta, double pgamma)
    {
      F1 = new List<double>(); F2 = new List<double>();
      Y = pY.ToList();
      L = new List<double> { Y.First() }; Y = Y.Skip(1).ToList();
      T = new List<double> { 0.0 };
      S = new List<double> { St(Y.First(), L.First(), GetSts(1, s)) };

      alpha = palpha; beta = pbeta; gamma = pgamma;

      int t = 2;
      foreach (double d in Y.Skip(1))
      {
        double Lt1 = L.Last();
        L.Add(Lt(d, GetSts(t, s), L.Last(), T.Last()));
        T.Add(Tt(L.Last(), Lt1, T.Last()));
        S.Add(St(d, L.Last(), GetSts(t, s)));
        t++;
      }
    }
    public static (double[], double[]) Calculate(int p, int s)
    {
      int t = 1;
      List<(double, double)> zip = L.Zip(T, (lz, tz) => (lz, tz)).ToList();
      foreach ((double, double) z in zip) { F1.Add(Ytp(z.Item1, 1, z.Item2, GetSts(t, s))); t++; }
      //Calculate until p == 0
      int index = p;
      while (index > 0) { F2.Add(Ytp(L.Last(), index, T.Last(), GetSts(t, s))); t++; --index; }

      return (F1.ToArray(), F2.ToArray());
    }
  }
}
