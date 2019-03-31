using Glue;
using Gtk;
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
    public static List<InventoryOutput> stdSimulation;
    public static List<ForecastStatisticsTableData> stdForecast;
    public static Window mainWindow = null;
    public static VBox box = null;
    private static (double, double) bestForecast;
    private static (double, double) bestSimulation;
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
          top = (uint)pvds.Count + 2,
          bottom = (uint)pvds.Count + 3
        };
        pvds.Add(pvd);
      });

      return pvds;
    }
    public static List<TableData> ForecastStatisticalTableBuilder()
    {
      if (null == stdForecast) throw new NullParameter();
      //TODO: throw new exception if left == right

      List<TableData> tds = new List<TableData>();

      stdForecast.ForEach(std =>
      {
        List<Label> lbls = new List<Label>
        {
          //new Label(std.GetSampleSize()),
          //new Label(std.GetPopulationSize()),
          new Label(std.GetMeanAbsoluteDeviation()),
          new Label(std.GetMeanAbsolutePercentageError()),
          new Label(std.GetMeanPercentageError()),
          new Label(std.GetMeanSquaredError()),
          new Label(std.GetRootMeanSquareDeviation()),
          new Label(std.GetNextPeriod())
        };

        VBox vbox = new VBox(true, 0);
        lbls.ForEach(l => { vbox.PackStart(new Alignment(1, 0, 0, 0) { l }, true, true, 0); });

        TableData td = new TableData
        {
          vbox = vbox,
          left = 3,
          right = 4,
          top = (uint)tds.Count + 2,
          bottom = (uint)tds.Count + 3
        };

        tds.Add(td);
      });

      return tds;
    }
    public static List<TableData> SimulationStatisticalTableBuilder()
    {
      if (null == stdSimulation) throw new NullParameter();
      //TODO: throw new exception if left == right

      List<TableData> tds = new List<TableData>();

      stdSimulation.ForEach(std =>
      {
        List<Label> lbls = new List<Label>
        {
          new Label(std.GetTotalDemand()),
          new Label(std.GetSatisfiedDemand()),
          new Label(std.GetMissedDemand()),
          new Label(std.GetOrderFitness()),
          //new Label(std.GetTboFitness()),
          new Label(std.GetNextPeriod())
        };

        VBox vbox = new VBox(true, 0);
        lbls.ForEach(l => { vbox.PackStart(new Alignment(0, 0, 0, 0) { l }, true, true, 0); });

        TableData td = new TableData
        {
          vbox = vbox,
          left = 2,
          right = 3,
          top = (uint)tds.Count + 2,
          bottom = (uint)tds.Count + 3
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

      //MenuItem set_default_options_item = new MenuItem("Opciones por defecto");
      //Button btnSave = new Button("Guardar");
      //set_default_options_item.Activated += new EventHandler(delegate (object o, EventArgs args) { DefaultOptions(btnSave).ShowAll(); });
      //file_menu.Append(set_default_options_item);

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
      Table tableLayout = new Table((uint)tableRows + 2, 6, false);
      Label lblProduct = new Label
      {
        Markup = "<span font-size='1' weight='bold'>" + Product.activeElement.Item2 + "</span>",
        UseMarkup = true
      };
      double amount = bestSimulation.Item2 > bestForecast.Item2 ? bestSimulation.Item2 : bestForecast.Item2;
      Label lblAmount = new Label()
      {
        Markup = "<span font-size='1' weight='bold'>" + amount + "</span>",
        UseMarkup = true
      };
      Label lblPeriod = new Label()
      {
        Markup = "<span font-size='1' weight='bold'>" + Product.period + "</span>",
        UseMarkup = true
      };
      tableLayout.Attach(lblProduct, 1, 2, 0, 1, AttachOptions.Expand, AttachOptions.Expand, 5, 5);
      tableLayout.Attach(lblAmount, 2, 4, 0, 1, AttachOptions.Shrink, AttachOptions.Expand, 5, 5);
      tableLayout.Attach(lblPeriod, 4, 5, 0, 1, AttachOptions.Expand, AttachOptions.Expand, 5, 5);
      Label lblSimulation = new Label("Simulación");
      Label lblOptions = new Label("Estadisticos/Datos");
      Label lblForecast = new Label("Pronóstico");
      tableLayout.Attach(lblSimulation, 0, 2, 1, 2, AttachOptions.Expand, AttachOptions.Shrink, 5, 5);
      tableLayout.Attach(lblOptions, 2, 4, 1, 2, AttachOptions.Shrink, AttachOptions.Shrink, 5, 5);
      tableLayout.Attach(lblForecast, 4, 6, 1, 2, AttachOptions.Expand, AttachOptions.Shrink, 5, 5);

      int tableSize = stbSimulation.Count >= stbForecast.Count ? stbSimulation.Count : stbForecast.Count;
      for (int i = 2; i < tableSize; i++)
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
      stdSimulation = Center.SimulationData(Product.activeElement);
      stdSimulation = stdSimulation.OrderBy(s => s.orderFitness).Reverse().ToList();
      bestSimulation =
        (Math.Round(stdSimulation.First().orderFitness, 3), Math.Round(stdSimulation.First().returnDoubles.Last(), 3));
      //Get data from forecasts
      List<((double[], double[]), string)> forecasts = Center.ForecastData(Product.activeElement);
      //Get stats data from forecasts
      stdForecast = Center.ForecastStatData();

      List<(((double[], double[]), string), ForecastStatisticsTableData)> sortedForecasts =
        forecasts.Zip(stdForecast, (f, s) => (f, s)).OrderBy(fs => fs.Item2.MeanAbsoluteDeviation).ToList();
      //Move simple average to the bottom
      if(sortedForecasts.First().Item1.Item2 ==  "Promedio simple")
      {
        var t = sortedForecasts.First();
        sortedForecasts = sortedForecasts.Skip(1).ToList();
        sortedForecasts.Add(t);
      }
      forecasts = sortedForecasts.Select(sf => sf.Item1).ToList();
      stdForecast = sortedForecasts.Select(sf => sf.Item2).ToList();
      bestForecast =
        (Math.Round(stdForecast.First().MeanAbsoluteDeviation, 3), Math.Round(stdForecast.First().NextPeriod, 3));

      List<PlotData> pdSimulation = new List<PlotData>();
      foreach (InventoryOutput s in stdSimulation)
      {
        pdSimulation.Add(new PlotData(s.name, s.returnDoubles));
      }

      List<PlotData> pdForecast = new List<PlotData>();
      foreach (((double[], double[]), string) f in forecasts) { pdForecast.Add(new PlotData(f.Item2, ArrayBased.Join(f.Item1))); }

      pvdsSimulation = PlotBuilder(pdSimulation, Utils.PlotType.Simulation);
      pvdsForecast = PlotBuilder(pdForecast, Utils.PlotType.Forecast);

      stbSimulation = SimulationStatisticalTableBuilder();
      stbForecast = ForecastStatisticalTableBuilder();
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
      string html = "<!DOCTYPE html>" +
        "<html>" +
        "<head>" +
        "<link href=\"https://fonts.googleapis.com/css?family=Crimson+Text\" rel=\"stylesheet\">" +
        "<link href=\"https://fonts.googleapis.com/css?family=Inconsolata\" rel=\"stylesheet\">" +
        "<style>" +
        "h1, h2 { text-align: center; font-family: \"Crimson Text\", serif; }" +
        "h2 { margin-bottom: -50px; }" +
        "img { width: 100%; margin-top: 75px; margin-bottom: 15px; }" +
        "div { font-family: \"Inconsolata\", monospace; }" +
        "body { margin: 60px 40px; }" +
        "</style>" +
        "</head>" +
        "<body>" +
        "<h1>Reporte</h1>";
      string path = Directory.GetCurrentDirectory();

      html += PngToPdf() + "</body>" +
        "</html>";

      using (FileStream fs = new FileStream("Reporte.html", FileMode.Create))
      {
        using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
        {
          w.WriteLine(html);
        }
      }
    }
    private static string PngToPdf()
    {
      string html = "";
      List<(PlotViewData, InventoryOutput)> zipListSim = new List<(PlotViewData, InventoryOutput)>();
      List<(PlotViewData, ForecastStatisticsTableData)> zipListFor= new List<(PlotViewData, ForecastStatisticsTableData)>();

      zipListSim = pvdsSimulation.Zip(stdSimulation, (p, s) => (p, s)).ToList();
      zipListFor = pvdsForecast.Zip(stdForecast, (p, s) => (p, s)).ToList();

      html += "<h2>Pronósticos</h2>";
      foreach ((PlotViewData, ForecastStatisticsTableData) z in zipListFor)
      {
        PlotViewData pvd = z.Item1; ForecastStatisticsTableData std = z.Item2;

        MemoryStream stream = new MemoryStream();
        var pngExporter = new PngExporter { Width = 700, Height = 250, Background = OxyColors.White };
        pngExporter.Export(pvd.plotView.Model, stream);
        byte[] pngBinaryData = stream.ToArray();
        string imgDataURI = @"data:image/png;base64," + Convert.ToBase64String(pngBinaryData);
        html += string.Format("<img src='{0}'>", imgDataURI);
        html += "<br>";

        html += std.GenerateHtml();
      }

      html += "<h2>Simulaciones</h2>";
      foreach ((PlotViewData, InventoryOutput) z in zipListSim)
      {
        PlotViewData pvd = z.Item1; InventoryOutput io = z.Item2;

        MemoryStream stream = new MemoryStream();
        var pngExporter = new PngExporter { Width = 700, Height = 250, Background = OxyColors.White };
        pngExporter.Export(pvd.plotView.Model, stream);
        byte[] pngBinaryData = stream.ToArray();
        string imgDataURI = @"data:image/png;base64," + Convert.ToBase64String(pngBinaryData);
        html += string.Format("<img src='{0}'>", imgDataURI);
        html += "<br>";

        html += io.GenerateHtml();
      }

      return html;
    }
    private static void ShowHelp()
    {
      Window window = new Window("Ayuda")
      {
        Modal = true
      };
      window.SetDefaultSize(820, 550);
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
