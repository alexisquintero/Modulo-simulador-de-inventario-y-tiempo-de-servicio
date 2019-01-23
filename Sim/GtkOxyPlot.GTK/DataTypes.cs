using Gtk;
using OxyPlot;
using OxyPlot.GtkSharp;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GtkOxyPlot.GTK
{
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
  public class PlotViewData
  {
    public PlotView plotView;
    public uint left;
    public uint right;
    public uint top;
    public uint bottom;
  }
}
