using System;
using System.Collections.Generic;
using System.Linq;
using Utils.Normalized;

namespace Utils
{
  public enum PlotType { Simulation, Forecast };
  public enum Period { Diario = 50, Mensual = 4, Anual = 2 };
  public enum Distributions { Normal, Poisson, Exponential, UniformCont, UniformDisc };
  public enum Generator { OrderSize, TimeBetweenOrders };
  public class ForecastStatisticsTableData
  {
    public ForecastStatisticsTableData(int ss, int ps, double mad, double mape, double mpe, double mse, double rmsd, DateTime sd, double np)
    {
      SampleSize = ss; PopulationSize = ps; MeanAbsoluteDeviation = mad; MeanAbsolutePercentageError = mape;
      MeanPercentageError = mpe; MeanSquaredError = mse; RootMeanSquareDeviation = rmsd; StartDate = sd;
      NextPeriod = np;
    }
    public int SampleSize;
    public string GetSampleSize() { return "Tamaño de la muestra: " + SampleSize.ToString(); }
    public int PopulationSize;
    public string GetPopulationSize() { return "Tamaño de la población: " + PopulationSize.ToString(); }
    public double MeanAbsoluteDeviation;
    public string GetMeanAbsoluteDeviation() { return "Desviación Media Absoluta: " + Math.Round(MeanAbsoluteDeviation, 3).ToString(); }
    public double MeanAbsolutePercentageError;
    public string GetMeanAbsolutePercentageError() { return "Desviación Media Porcentual: " + Math.Round(MeanAbsolutePercentageError, 3).ToString(); }
    public double MeanPercentageError;
    public string GetMeanPercentageError() { return "Error Porcentual Medio: " + Math.Round(MeanPercentageError, 3).ToString(); }
    public double MeanSquaredError;
    public string GetMeanSquaredError() { return "Error Cuadrático Medio: " + Math.Round(MeanSquaredError, 3).ToString(); }
    public double RootMeanSquareDeviation;
    public string GetRootMeanSquareDeviation() { return "Raíz cuadrada del error cuadrático medio: " + Math.Round(RootMeanSquareDeviation, 3).ToString(); }
    public DateTime StartDate;
    public string GetStartDate() { return "Fecha de inicio: " + StartDate.ToShortDateString().ToString(); }
    public double NextPeriod;
    public string GetNextPeriod() { return "Próximo período: " + Math.Round(NextPeriod, 3).ToString(); }
    public string GenerateHtml()
    {
      string html = "<div>" + //GetSampleSize() + "</span><br>" +
        //GetPopulationSize() + "</span><br>" +
        GetMeanAbsoluteDeviation() + "</span><br>" +
        GetMeanAbsolutePercentageError() + "</span><br>" +
        GetMeanPercentageError() + "</span><br>" +
        GetMeanSquaredError() + "</span><br>" +
        GetRootMeanSquareDeviation() + "</span><br>" +
        GetStartDate() + "</span><br>" +
        GetNextPeriod() + "</span><br>" +
        "</div>";
      return html;
    }
  }
  public class DefaultOptions
  {
    public int sampleSize = 0;
    public DateTime startDate = DateTime.Today;
  }
  public class InventoryOutput
  {
    public string name = "Simulación de Inventario ";
    public double totalDemand = 0;
    public string GetTotalDemand() { return "Demanda total: " + Math.Round(totalDemand, 3).ToString(); }
    public double satisfiedDemand = 0;
    public string GetSatisfiedDemand() { return "Demanda satisfacida: " + Math.Round(satisfiedDemand, 3).ToString(); }
    public double missedDemand = 0;
    public string GetMissedDemand() { return "Demanda no satisfacida: " + Math.Round(missedDemand, 3).ToString(); }
    public List<(DateTime, double)> returnData;
    public double[] returnDoubles;
    public double orderFitness;
    public string GetOrderFitness() { return "Órden fitness: " + Math.Round(orderFitness, 3).ToString(); }
    public double tboFitness;
    public string GetTboFitness() { return "Tiempo entre órdenes fitness: " + Math.Round(tboFitness, 3).ToString(); }
    private static List<(DateTime, double)> realValues;
    public string GetNextPeriod() { return "Próximo período: " + Math.Round(returnDoubles.Last(), 3).ToString(); }
    public InventoryOutput(double t, double s, double m, List<(DateTime, double)> r, List<(DateTime, double)> rv,
      Period p, Distributions orderAmmount, Distributions timeBetweenOrders, double ocod, double tbocod)
    { totalDemand = t; satisfiedDemand = s; missedDemand = m; returnData = r; realValues = rv;
      returnDoubles = ProcessSimulationOutput(r, p);
      name += "(" + orderAmmount.ToString() + "|" + timeBetweenOrders.ToString() + ")"; orderFitness = ocod;
      tboFitness = tbocod;
    }
    public string GenerateHtml()
    {
      string html = "<div>" + 
        GetTotalDemand() + "</span><br>" +
        GetSatisfiedDemand() + "</span><br>" +
        GetMissedDemand() + "</span><br>" +
        GetOrderFitness() + "</span><br>" +
        //GetTboFitness() + "</span><br>" +
        GetNextPeriod() + "</span><br>" +
        "</div>";
      return html;
    }
    private static double[] ProcessSimulationOutput(List<(DateTime, double)> simData, Period period)
    {
      List<(DateTime, double)> groupedData = new List<(DateTime, double)>();
      switch (period)
      {
        case Period.Diario:
          groupedData = simData.GroupBy(
            s => new DateTime(s.Item1.Year, s.Item1.Month, s.Item1.Day),
            s => s.Item2,
            (date, doub) => (date, doub.Sum())
          ).OrderBy(g => g.Item1).ToList();
          break;
        case Period.Mensual:
          groupedData = simData.GroupBy(
            s => new DateTime(s.Item1.Year, s.Item1.Month, 1),
            s => s.Item2,
            (date, doub) => (date, doub.Sum())
          ).OrderBy(g => g.Item1).ToList();
          break;
        case Period.Anual:
          groupedData = simData.GroupBy(
            s => new DateTime(s.Item1.Year, 1, 1),
            s => s.Item2,
            (date, doub) => (date, doub.Sum())
          ).OrderBy(g => g.Item1).ToList();
          break;
        default: break;
      }

      if (groupedData.Count > 0)
      {
        //Check data starts and end same dates as real values
        switch (period)
        {
          case Period.Diario:
            {
              DateTime fSim = groupedData.First().Item1;
              DateTime fReal = realValues.First().Item1;

              if (fSim.Year != fReal.Year || fSim.Month != fReal.Month || fSim.Day != fReal.Day)
              {
                groupedData.Insert(0, (new DateTime(fReal.Year, fReal.Month, fReal.Day), 0));
              }

              DateTime lSim = groupedData.Last().Item1;
              DateTime lReal = realValues.Last().Item1.AddDays(1);

              while (lSim.Subtract(lReal).TotalSeconds > 0)
              {
                groupedData = groupedData.Take(groupedData.Count - 1).ToList();
                lSim = groupedData.Last().Item1;
              }
              if (lSim.Year != lReal.Year || lSim.Month != lReal.Month || lSim.Day != lReal.Day)
              {
                groupedData.Add((new DateTime(lReal.Year, lReal.Month, lReal.Day), 0));
              }
            }
            break;
          case Period.Mensual:
            {
              DateTime fSim = groupedData.First().Item1;
              DateTime fReal = realValues.First().Item1;

              if (fSim.Year != fReal.Year || fSim.Month != fReal.Month)
              {
                groupedData.Insert(0, (new DateTime(fReal.Year, fReal.Month, 1), 0));
              }

              DateTime lSim = groupedData.Last().Item1;
              DateTime lReal = realValues.Last().Item1.AddMonths(1);

              while (lSim.Subtract(lReal).TotalSeconds > 0)
              {
                groupedData = groupedData.Take(groupedData.Count - 1).ToList();
                lSim = groupedData.Last().Item1;
              }
              if (lSim.Year != lReal.Year || lSim.Month != lReal.Month)
              {
                groupedData.Add((new DateTime(lReal.Year, lReal.Month, 1), 0));
              }
            }
            break;
          case Period.Anual:
            {
              DateTime fSim = groupedData.First().Item1;
              DateTime fReal = realValues.First().Item1;

              if (fSim.Year != fReal.Year)
              {
                groupedData.Insert(0, (new DateTime(fReal.Year, 1, 1), 0));
              }

              DateTime lSim = groupedData.Last().Item1;
              DateTime lReal = realValues.Last().Item1.AddYears(1);

              while (lSim.Subtract(lReal).TotalSeconds > 0)
              {
                groupedData = groupedData.Take(groupedData.Count - 1).ToList();
                lSim = groupedData.Last().Item1;
              }
              if (lSim.Year != lReal.Year)
              {
                groupedData.Add((new DateTime(lReal.Year, 1, 1), 0));
              }
            }
            break;
          default: break;
        }
      } else return new List<double>() { 0.0 }.ToArray();

      List<(DateTime, double)> zeroData = Zerolized.AddZeroValue(groupedData, period);

      List<double> onlyDoubles = new List<double>();
      foreach ((DateTime, double) z in zeroData) { onlyDoubles.Add(z.Item2); }

      return onlyDoubles.ToArray();
    }
  }
}
