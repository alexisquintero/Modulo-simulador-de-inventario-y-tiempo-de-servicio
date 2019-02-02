using Glue;
using Gtk;
using IronPdf;
using OxyPlot;
using OxyPlot.GtkSharp;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Utils;
using Utils.Exceptions;

namespace GtkOxyPlot.GTK
{
  public class Helper
  {
    public static List<PlotViewData> pvdsSimulation;
    public static List<PlotViewData> pvdsForecast;
    public static List<TableData> stbSimulation;
    public static List<TableData> stbForecast;
    public static List<StatisticsTableData> stdSimulation;
    public static List<StatisticsTableData> stdForecast;
    public static Window mainWindow = null;
    public static VBox box = null;
    private static List<PlotView> PlotViewBuilder(List<PlotData> pds)
    {
      List<PlotView> pvs = new List<PlotView>();
      pds.ForEach(p =>
      {
        LineSeries ls = new LineSeries();
        ls.Points.AddRange(p.GetDataPoints());
        PlotModel plotModel = new PlotModel
        {
          Title = p.title
        };
        plotModel.Series.Add(ls);
        PlotView plotView = new PlotView
        {
          Model = plotModel
        };
        pvs.Add(plotView);
      });
      return pvs;
    }
    //Simulation plot occupy: left=0; right=2;
    //Forecast plot occupy: left=4; right=6;
    //Call first with Simulation plots, then Forecast, or vice versa
    public static List<PlotViewData> PlotBuilder(List<PlotData> pds, Utils.PlotType pt)
    {
      if (null == pds) throw new NullParameter();
      List<PlotView> pvs = PlotViewBuilder(pds);

      uint left, right;
      switch (pt)
      {
        case Utils.PlotType.Simulation: left = 0; right = 2; break;
        case Utils.PlotType.Forecast: left = 4; right = 6; break;
        default: left = 0; right = 0; break;
      }
      //TODO: throw new exception if left == right

      List<PlotViewData> pvds = new List<PlotViewData>();
      pvs.ForEach(pv =>
      {
        PlotViewData pvd = new PlotViewData
        {
          plotView = pv,
          left = left,
          right = right,
          top = (uint)pvds.Count + 1,
          bottom = (uint)pvds.Count + 2
        };
        pvds.Add(pvd);
      });

      return pvds;
    }
    public static List<TableData> StatisticalTableBuilder(Utils.PlotType tt)
    {
      uint left, right;
      float xalign;
      List<StatisticsTableData> stds = null;
      switch (tt)
      {
        case Utils.PlotType.Simulation: left = 2; right = 3; xalign = 0; stds = stdSimulation; break;
        case Utils.PlotType.Forecast: left = 3; right = 4; xalign = 1; stds = stdForecast; break;
        default: left = 0; right = 0; xalign = 0; break;
      }
      if (null == stds) throw new NullParameter();
      //TODO: throw new exception if left == right

      List<TableData> tds = new List<TableData>();

      stds.ForEach(std =>
      {
        List<Label> lbls = new List<Label>
        {
          new Label(std.GetSampleSize()),
          new Label(std.GetPopulationSize()),
          new Label(std.GetMeanAbsoluteDeviation()),
          new Label(std.GetMeanAbsolutePercentageError()),
          new Label(std.GetMeanPercentageError()),
          new Label(std.GetMeanSquaredError()),
          new Label(std.GetRootMeanSquareDeviation()),
          new Label(std.GetNextPeriod())
        };

        VBox vbox = new VBox(true, 0);
        uint lblsCounter = 0;
        lbls.ForEach(l =>
        {
          vbox.PackStart(new Alignment(xalign, 0, 0, 0) { l }, true, true, 0);
          lblsCounter++;
        });

        TableData td = new TableData
        {
          vbox = vbox,
          left = left,
          right = right,
          top = (uint)tds.Count + 1,
          bottom = (uint)tds.Count + 2
        };

        tds.Add(td);
      });

      return tds;
    }
    private static Window DefaultOptions(Button btnSave)
    {
      Window defOptions = new Window("Default Options")
      {
        Modal = true
      };
      defOptions.SetDefaultSize(250, 80);
      btnSave.Label = "Guardar";
      string dateDefaultValue = "dd/mm/yyyy";

      Table tableLayout = new Table(3, 2, false);
      Label lblSampleSize = new Label("Tamaño de la muestra");
      tableLayout.Attach(lblSampleSize, 0, 1, 0, 1);
      Entry entSampleSize = new Entry();
      tableLayout.Attach(entSampleSize, 1, 2, 0, 1);
      Label lblStartDate = new Label("Fecha de inicio");
      tableLayout.Attach(lblStartDate, 0, 1, 1, 2);
      Entry entStartDate = new Entry
      {
        Text = dateDefaultValue
      };
      tableLayout.Attach(entStartDate, 1, 2, 1, 2);

      btnSave.Pressed += new EventHandler(delegate (object o, EventArgs args)
      {
        int sampleSize = 0;
        bool sampleSizeEmpty = 0 == entSampleSize.Text.ToString().Trim().Length;
        bool sampleSizeParse = (!sampleSizeEmpty) ? int.TryParse(entSampleSize.Text, out sampleSize) : true;
        DateTime startDate = DateTime.Today;
        bool startDateEmpty = dateDefaultValue == entStartDate.Text.ToString();
        bool startDateParse = (!startDateEmpty) ? DateTime.TryParse(entStartDate.Text, out startDate) : true;

        string errorMessage = "";
        errorMessage += (!sampleSizeParse) ? "Tamaño de la muestra debe ser un número entero\n" : "";
        errorMessage += (!startDateParse) ? "Fecha de inicio debe ser de la forma " + dateDefaultValue + "\n" : "";

        if(!sampleSizeParse || !startDateParse)
        {
          MessageDialog messageDialog = new MessageDialog(defOptions, DialogFlags.Modal, MessageType.Error, ButtonsType.Close, errorMessage);
          messageDialog.Run();
          messageDialog.Destroy();
        } else
        {
          DefaultOptions defaultOptions = new DefaultOptions
          {
            sampleSize = sampleSize,
            startDate = startDate
          };
          (List<PlotViewData>, List<PlotViewData>, List<TableData>, List<TableData>) data = GatherData(defaultOptions);
          defOptions.Destroy();
          InitWindow();
        }
      });

      tableLayout.Attach(btnSave, 0, 2, 2, 3);

      defOptions.Add(tableLayout);

      return defOptions;
    }
    public static void InitWindow()
    {
      if(null == mainWindow) mainWindow = new Window("Plots");
      mainWindow.Destroyed += new EventHandler(delegate (object o, EventArgs args) { Application.Quit(); });
      mainWindow.SetDefaultSize(300, 200);
      mainWindow.Maximize();
      VBox vbox = new VBox(false, 2);

      MenuBar mb = new MenuBar();
      Menu file_menu = new Menu();

      MenuItem set_default_options_item = new MenuItem("Opciones por defecto");
      Button btnSave = new Button("Guardar");
      set_default_options_item.Activated += new EventHandler(delegate (object o, EventArgs args) { DefaultOptions(btnSave).ShowAll(); });
      file_menu.Append(set_default_options_item);

      MenuItem product_item = new MenuItem("Cambio de opciones iniciales");
      product_item.Activated += new EventHandler(delegate (object o, EventArgs args) { mainWindow.HideAll(); Product.mainWindow.ShowAll(); });
      file_menu.Append(product_item);

      MenuItem report_item = new MenuItem("Reporte");
      report_item.Activated += new EventHandler(delegate (object o, EventArgs args) { Report(); });
      file_menu.Append(report_item);

      MenuItem exit_item = new MenuItem("Salir");
      exit_item.Activated += new EventHandler(delegate (object o, EventArgs args) { Application.Quit(); });
      file_menu.Append(exit_item);

      MenuItem file_item = new MenuItem("Archivo")
      {
        Submenu = file_menu
      };
      mb.Append(file_item);

      Menu help_menu = new Menu();

      MenuItem help_window_item = new MenuItem("Ayuda");
      help_window_item.Activated += new EventHandler(delegate (object o, EventArgs args) { ShowHelp(); });
      help_menu.Append(help_window_item);

      MenuItem about_item = new MenuItem("Acerca");
      about_item.Activated += new EventHandler(delegate (object o, EventArgs args) { ShowAbout(); });
      help_menu.Append(about_item);

      MenuItem help_item = new MenuItem("Ayuda")
      {
        Submenu = help_menu
      };
      mb.Append(help_item);

      vbox.PackStart(mb, false, false, 0);

      int tableRows = pvdsForecast.Count > pvdsSimulation.Count ? pvdsForecast.Count : pvdsSimulation.Count;
      Table tableLayout = new Table((uint)tableRows + 1, 6, false);
      Label lblSimulation = new Label("Simulación");
      Label lblOptions = new Label("Estadisticos/Datos");
      Label lblForecast = new Label("Pronóstico");
      tableLayout.Attach(lblSimulation, 0, 2, 0, 1, AttachOptions.Expand, AttachOptions.Shrink, 5, 5);
      tableLayout.Attach(lblOptions, 2, 4, 0, 1, AttachOptions.Shrink, AttachOptions.Shrink, 5, 5);
      tableLayout.Attach(lblForecast, 4, 6, 0, 1, AttachOptions.Expand, AttachOptions.Shrink, 5, 5);

      int tableSize = stbSimulation.Count >= stbForecast.Count ? stbSimulation.Count : stbForecast.Count;
      for (int i = 1; i < tableSize; i++)
      {
        tableLayout.SetRowSpacing((uint)i, 20);
      }
      int width = 1280;
      int length = stbSimulation.Count * 300;
      tableLayout.SetSizeRequest(width, length);

      pvdsSimulation.ForEach(pvd => tableLayout.Attach(pvd.plotView, pvd.left, pvd.right, pvd.top, pvd.bottom));
      pvdsForecast.ForEach(pvd => tableLayout.Attach(pvd.plotView, pvd.left, pvd.right, pvd.top, pvd.bottom));

      stbSimulation.ForEach(td => tableLayout.Attach(td.vbox, td.left, td.right, td.top, td.bottom, AttachOptions.Fill, AttachOptions.Shrink, 10, 0));
      stbForecast.ForEach(td => tableLayout.Attach(td.vbox, td.left, td.right, td.top, td.bottom, AttachOptions.Fill, AttachOptions.Shrink, 10, 0));

      ScrolledWindow sw = new ScrolledWindow();
      sw.AddWithViewport(tableLayout);
      vbox.PackStart(sw, true, true, 0);
      if (null != box) mainWindow.Remove(box);
      box = vbox;
      mainWindow.Add(vbox);
      mainWindow.ShowAll();
    }
    public static (List<PlotViewData>, List<PlotViewData>, List<TableData>, List<TableData>) GatherData(DefaultOptions defaultOptions)
    {
      //Set period
      Center.period = Product.period;

      //Get data from simulations
      List<(string, double[])> simulations = Center.SimulationData(Product.activeElement);
      //Get stats data from simulations
      //Get data from forecasts
      List<(string, (double[], double[]))> forecasts = Center.ForecastData(Product.activeElement);
      //Get stats data from forecasts
      stdForecast = Center.ForecastStatData();

      List<PlotData> pdSimulation = new List<PlotData>();
      foreach ((string, double[]) s in simulations) { pdSimulation.Add(new PlotData(s.Item1, s.Item2)); }

      List<PlotData> pdForecast = new List<PlotData>();
      foreach ((string, (double[], double[])) f in forecasts) { pdForecast.Add(new PlotData(f.Item1, ArrayBased.Join(f.Item2))); }

      pvdsSimulation = PlotBuilder(pdSimulation, Utils.PlotType.Simulation);
      pvdsForecast = PlotBuilder(pdForecast, Utils.PlotType.Forecast);

      StatisticsTableData std11 = new StatisticsTableData(defaultOptions.sampleSize, 2, 3, 4, 5, 6, 7, defaultOptions.startDate, 99);
      StatisticsTableData std12 = new StatisticsTableData(defaultOptions.sampleSize, 2, 3, 4, 5, 6, 7, defaultOptions.startDate, 99);
      StatisticsTableData std13 = new StatisticsTableData(defaultOptions.sampleSize, 2, 3, 4, 5, 6, 7, defaultOptions.startDate, 99);
      StatisticsTableData std14 = new StatisticsTableData(defaultOptions.sampleSize, 2, 3, 4, 5, 6, 7, defaultOptions.startDate, 99);
      stdSimulation = new List<StatisticsTableData>
      {
        std11,
        std12,
        std13,
        std14
      };

      stbSimulation = StatisticalTableBuilder(Utils.PlotType.Simulation);
      stbForecast = StatisticalTableBuilder(Utils.PlotType.Forecast);
      return (pvdsSimulation, pvdsForecast, stbSimulation, stbForecast);
    }
    private static void ShowAbout()
    {
      Window window = new Window("About")
      {
        Modal = true
      };
      window.SetDefaultSize(250, 70);
      Table table = new Table(2, 1, true);
      Label proyect = new Label("Proyecto: Modulo simulador de inventario");
      Label author = new Label("Autor: Alexis Quintero");

      table.Attach(proyect, 0, 1, 0, 1);
      table.Attach(author, 0, 1, 1, 2);
      window.Add(table);

      window.ShowAll();
    }
    private static void Report()
    {
      HtmlToPdf Renderer = new HtmlToPdf();
      string html = "<h1>Reporte</h1>";
      string path = Directory.GetCurrentDirectory();

      html += PngToPdf(Utils.PlotType.Simulation);
      html += PngToPdf(Utils.PlotType.Forecast);

      PdfDocument pdfdoc = Renderer.RenderHtmlAsPdf(html);
      pdfdoc.SaveAs("Reporte.pdf");
    }
    private static void DeleteFiles()
    {
      int i = 0;
      string path = Directory.GetCurrentDirectory();
      //Delete simulation plots pngs
      foreach (PlotViewData pvd in pvdsSimulation)
      {
        string thisPath = path + "//plot_simulation_" + i.ToString() + ".png";
        if(File.Exists(thisPath))
        {
          File.Delete(thisPath);
        }
        i++;
      }
      //Delete forecast plots pngs
      i = 0;
      foreach (PlotViewData pvd in pvdsForecast)
      {
        string thisPath = path + "//plot_forecast_" + i.ToString() + ".png";
        if(File.Exists(thisPath))
        {
          File.Delete(thisPath);
        }
        i++;
      }
    }
    private static string PngToPdf(Utils.PlotType plotType)
    {
      PngExporter pngExporter = new PngExporter { Width = 700, Height = 400, Background = OxyColors.White };
      string html = "";
      int index = 0;
      string fileName = "plot_";
      List<(PlotViewData, StatisticsTableData)> zipList = new List<(PlotViewData, StatisticsTableData)>();

      switch (plotType)
      {
        case Utils.PlotType.Simulation:
          fileName += "simulation_";
          zipList = pvdsSimulation.Zip(stdSimulation, (p, s) => (p, s)).ToList();
          break;
        case Utils.PlotType.Forecast: fileName += "forecast_";
          zipList = pvdsForecast.Zip(stdForecast, (p, s) => (p, s)).ToList();
          break;
        default: break;
      }

      foreach ((PlotViewData, StatisticsTableData) z in zipList)
      {
        PlotViewData pvd = z.Item1; StatisticsTableData std = z.Item2;

        Stream stream = new FileStream(fileName + index.ToString() + ".png", FileMode.Create, FileAccess.Write, FileShare.None);
        pngExporter.Export(pvd.plotView.Model, stream);
        stream.Close();
        byte[] pngBinaryData = File.ReadAllBytes(fileName + index.ToString() + ".png");
        string imgDataURI = @"data:image/png;base64," + Convert.ToBase64String(pngBinaryData);
        html += String.Format("<img src='{0}'>", imgDataURI);
        html += "<br/>";

        html += "<div>" +
          std.GetSampleSize() + "</span><br>" +
          std.GetPopulationSize() + "</span><br>" +
          std.GetMeanAbsoluteDeviation() + "</span><br>" +
          std.GetMeanAbsolutePercentageError() + "</span><br>" +
          std.GetMeanPercentageError() + "</span><br>" +
          std.GetMeanSquaredError() + "</span><br>" +
          std.GetRootMeanSquareDeviation() + "</span><br>" +
          std.GetStartDate() + "</span><br>" +
          std.GetNextPeriod() + "</span><br>" +
          "</div>";
        index++;
      }
      DeleteFiles();
      return html;
    }
    private static void ShowHelp()
    {
      Window window = new Window("Ayuda")
      {
        Modal = true
      };
      window.SetDefaultSize(810, 550);
      string text;
      FileStream fileStream = new FileStream(@"TempHelp.txt", FileMode.Open, FileAccess.Read);
      using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
      {
        text = streamReader.ReadToEnd();
      };
      TextView textView = new TextView()
      {
        Editable = false
      };
      textView.Buffer.Text = text;
      Table table = new Table(1, 1, true);
      ScrolledWindow sw = new ScrolledWindow();
      table.Add(textView);
      sw.AddWithViewport(table);
      window.Add(sw);
      window.ShowAll();
    }
  }
}
