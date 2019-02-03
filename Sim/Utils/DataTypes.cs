using System;
using System.Collections.Generic;
using System.Text;

namespace Utils
{
  public enum PlotType { Simulation, Forecast };
  public enum Period { Diario = 50, Mensual = 4, Anual = 2};
  public enum Distributions { Normal, Poisson, Exponential, UniformCont, UniformDisc };
  public enum Generator { OrderSize, TimeBetweenOrders };
  public class StatisticsTableData
  {
    public StatisticsTableData(int ss, int ps, double mad, double mape, double mpe, double mse, double rmsd, DateTime sd, double np)
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
  }
  public class DefaultOptions
  {
    public int sampleSize = 0;
    public DateTime startDate = DateTime.Today;
  }
}
