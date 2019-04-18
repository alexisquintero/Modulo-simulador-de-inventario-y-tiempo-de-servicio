using System.Collections.Generic;
using System.Linq;

namespace Forecast.Method.AverageBased
{
  class WintersHelper
  {
    //131
    private static List<decimal> Y = new List<decimal>();
    private static List<decimal> L = new List<decimal>();
    private static List<decimal> T = new List<decimal>();
    private static List<decimal> S = new List<decimal>();
    private static List<decimal> F1 = new List<decimal>();
    private static List<decimal> F2 = new List<decimal>();
    private static decimal alpha;
    private static decimal beta;
    private static decimal gamma;
    public static decimal Lt(decimal Yt, decimal Sts, decimal Lt1,decimal Tt1)
    { return alpha * Yt / Sts + (1 - alpha) * (Lt1 + Tt1); }
    public static decimal Tt(decimal Lt, decimal Lt1, decimal Tt1)
    { return beta * (Lt - Lt1) + (1 - beta) * Tt1; }
    public static decimal St(decimal Yt, decimal Lt, decimal Sts)
    { return gamma * Yt / Lt + (1 - gamma) * Sts; }
    public static decimal Ytp(decimal Lt, int p, decimal Tt, decimal Stsp)
    { return (Lt + p * Tt) * Stsp; }
    private static decimal GetSts(int t, int s)
    {
      int index = (t - s) - 1;
      return index < 0 ? 1.0m : S.ElementAt(index);
    }
    public static void Init(double[] pY, int s, decimal palpha, decimal pbeta, decimal pgamma)
    {
      F1 = new List<decimal>(); F2 = new List<decimal>();
      Y = pY.Select(d => (decimal)d).ToList();
      L = new List<decimal> { Y.First() }; Y = Y.Skip(1).ToList();
      T = new List<decimal> { 0.0m };
      S = new List<decimal> { St(Y.First(), L.First(), GetSts(1, s)) };

      alpha = palpha; beta = pbeta; gamma = pgamma;

      int t = 2;
      foreach (decimal d in Y.Skip(1))
      {
        decimal Lt1 = L.Last();
        L.Add(Lt(d, GetSts(t, s), L.Last(), T.Last()));
        T.Add(Tt(L.Last(), Lt1, T.Last()));
        S.Add(St(d, L.Last(), GetSts(t, s)));
        t++;
      }
    }
    public static (double[], double[]) Calculate(int p, int s)
    {
      int t = 1;
      List<(decimal, decimal)> zip = L.Zip(T, (lz, tz) => (lz, tz)).ToList();
      foreach ((decimal, decimal) z in zip) { F1.Add(Ytp(z.Item1, 1, z.Item2, GetSts(t, s))); t++; }
      //Calculate until p == 0
      int index = p;
      while (index > 0) { F2.Add(Ytp(L.Last(), index, T.Last(), GetSts(t, s))); t++; --index; }

      return (F1.Select(m => (double)m).ToArray(), F2.Select(m => (double)m).ToArray());
    }
  }
}
