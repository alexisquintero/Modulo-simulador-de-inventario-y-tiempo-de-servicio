using Gtk;
using OxyPlot;
using OxyPlot.GtkSharp;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GtkOxyPlot.GTK
{
  public enum PlotType { Simulation, Forecast };
  public class PlotData
  {
    public PlotData(string title, double[] rd)
    {
      this.title = title; rawData = rd;
    }
    public string title;
    private double[] rawData = new double[] { };
    public LineSeries data = new LineSeries();

    public List<DataPoint> GetDataPoints()
    {
      return Enumerable.Range(0, rawData.Length).Zip(rawData, (x, y) => new DataPoint(x, y)).ToList();
    }
  }
  public class TableData
  {
    //public Table table;
    public VBox vbox;
    public uint left;
    public uint right;
    public uint top;
    public uint bottom;
  }
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
    public string GetMeanAbsoluteDeviation() { return "Desviación Media Absoluta: " + MeanAbsoluteDeviation.ToString(); }
    public double MeanAbsolutePercentageError;
    public string GetMeanAbsolutePercentageError() { return "Desviación Media Porcentual: " + MeanAbsolutePercentageError.ToString(); }
    public double MeanPercentageError;
    public string GetMeanPercentageError() { return "Error Porcentual Medio: " + MeanPercentageError.ToString(); }
    public double MeanSquaredError;
    public string GetMeanSquaredError() { return "Error Cuadrático Medio: " + MeanSquaredError.ToString(); }
    public double RootMeanSquareDeviation;
    public string GetRootMeanSquareDeviation() { return "Raíz cuadrada del error cuadrático medio: " + RootMeanSquareDeviation.ToString(); }
    public DateTime StartDate;
    public string GetStartDate() { return "Fecha de inicio: " + StartDate.ToShortDateString().ToString(); }
    public double NextPeriod;
    public string GetNextPeriod() { return "Próximo período: " + NextPeriod.ToString(); }
  }
  public class PlotViewData
  {
    public PlotView plotView;
    public uint left;
    public uint right;
    public uint top;
    public uint bottom;
  }
  public class DefaultOptions
  {
    public int sampleSize = 0;
    public DateTime startDate = DateTime.Today;
  }
}
