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
    public static List<PlotViewData> PlotBuilder(List<PlotData> pds, PlotType pt)
    {
      if (null == pds) throw new NullParameter();
      List<PlotView> pvs = PlotViewBuilder(pds);

      uint left, right;
      switch (pt)
      {
        case PlotType.Simulation: left = 0; right = 2; break;
        case PlotType.Forecast: left = 4; right = 6; break;
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
    public static List<TableData> StatisticalTableBuilder(PlotType tt)
    {
      uint left, right;
      float xalign;
      List<StatisticsTableData> stds = null;
      switch (tt)
      {
        case PlotType.Simulation: left = 2; right = 3; xalign = 0; stds = stdSimulation; break;
        case PlotType.Forecast: left = 3; right = 4; xalign = 1; stds = stdForecast; break;
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
          InitWindow(data.Item1, data.Item2, data.Item3, data.Item4);
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

      MenuItem product_item = new MenuItem("Cambio de producto");
      product_item.Activated += new EventHandler(delegate (object o, EventArgs args) { mainWindow.HideAll(); Product.mainWindow.ShowAll(); });
      file_menu.Append(product_item);

      MenuItem report_item = new MenuItem("Reporte");
      report_item.Activated += new EventHandler(delegate (object o, EventArgs args) { Report(); });
      file_menu.Append(report_item);

      MenuItem exit_item = new MenuItem("Salir");
      exit_item.Activated += new EventHandler(delegate (object o, EventArgs args) { Application.Quit(); });
      file_menu.Append(exit_item);

      MenuItem file_item = new MenuItem("Archivos")
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

      Table tableLayout = new Table(5, 6, false);
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
    public static void InitWindow(List<PlotViewData> pvdsS, List<PlotViewData> pvdsF, List<TableData> stbS, List<TableData> stbF)
    {
      pvdsSimulation = pvdsS;
      pvdsForecast = pvdsF;
      stbSimulation = stbS;
      stbForecast = stbF;
      InitWindow();
    }
    public static (List<PlotViewData>, List<PlotViewData>, List<TableData>, List<TableData>) GatherData(DefaultOptions defaultOptions)
    {
      //Table[{cosine},{x,0,10,0.1}] on WolframAlpha
      double[] cos = { 1, 0.995004, 0.980067, 0.955336, 0.921061, 0.877583, 0.825336, 0.764842, 0.696707, 0.62161, 0.540302, 0.453596, 0.362358, 0.267499, 0.169967, 0.0707372, -0.0291995, -0.128844, -0.227202, -0.32329, -0.416147, -0.504846, -0.588501, -0.666276, -0.737394, -0.801144, -0.856889, -0.904072, -0.942222, -0.970958, -0.989992, -0.999135, -0.998295, -0.98748, -0.966798, -0.936457, -0.896758, -0.8481, -0.790968, -0.725932, -0.653644, -0.574824, -0.490261, -0.400799, -0.307333, -0.210796, -0.112153, -0.0123887, 0.087499, 0.186512, 0.283662, 0.377978, 0.468517, 0.554374, 0.634693, 0.70867, 0.775566, 0.834713, 0.88552, 0.927478, 0.96017, 0.983268, 0.996542, 0.999859, 0.993185, 0.976588, 0.950233, 0.914383, 0.869397, 0.815725, 0.753902, 0.684547, 0.608351, 0.526078, 0.438547, 0.346635, 0.25126, 0.153374, 0.0539554, -0.0460021, -0.1455, -0.243544, -0.339155, -0.431377, -0.519289, -0.602012, -0.67872, -0.748647, -0.811093, -0.865435, -0.91113, -0.947722, -0.974844, -0.992225, -0.999693, -0.997172, -0.984688, -0.962365, -0.930426, -0.889191, -0.839072 };
      double[] sin = { 0, 0.0998334, 0.198669, 0.29552, 0.389418, 0.479426, 0.564642, 0.644218, 0.717356, 0.783327, 0.841471, 0.891207, 0.932039, 0.963558, 0.98545, 0.997495, 0.999574, 0.991665, 0.973848, 0.9463, 0.909297, 0.863209, 0.808496, 0.745705, 0.675463, 0.598472, 0.515501, 0.42738, 0.334988, 0.239249, 0.14112, 0.0415807, -0.0583741, -0.157746, -0.255541, -0.350783, -0.44252, -0.529836, -0.611858, -0.687766, -0.756802, -0.818277, -0.871576, -0.916166, -0.951602, -0.97753, -0.993691, -0.999923, -0.996165, -0.982453, -0.958924, -0.925815, -0.883455, -0.832267, -0.772764, -0.70554, -0.631267, -0.550686, -0.464602, -0.373877, -0.279415, -0.182163, -0.0830894, 0.0168139, 0.116549, 0.21512, 0.311541, 0.40485, 0.494113, 0.57844, 0.656987, 0.728969, 0.793668, 0.850437, 0.898708, 0.938, 0.96792, 0.988168, 0.998543, 0.998941, 0.989358, 0.96989, 0.940731, 0.902172, 0.854599, 0.798487, 0.734397, 0.662969, 0.584917, 0.501021, 0.412118, 0.319098, 0.22289, 0.124454, 0.0247754, -0.0751511, -0.174327, -0.271761, -0.366479, -0.457536, -0.544021 };
      double[] cosh = { 1, 1.005, 1.02007, 1.04534, 1.08107, 1.12763, 1.18547, 1.25517, 1.33743, 1.43309, 1.54308, 1.66852, 1.81066, 1.97091, 2.1509, 2.35241, 2.57746, 2.82832, 3.10747, 3.41773, 3.7622, 4.14431, 4.56791, 5.03722, 5.55695, 6.13229, 6.76901, 7.47347, 8.25273, 9.11458, 10.0677, 11.1215, 12.2866, 13.5748, 14.9987, 16.5728, 18.3128, 20.236, 22.3618, 24.7113, 27.3082, 30.1784, 33.3507, 36.8567, 40.7316, 45.0141, 49.7472, 54.9781, 60.7593, 67.1486, 74.2099, 82.014, 90.6389, 100.171, 110.705, 122.348, 135.215, 149.435, 165.151, 182.52, 201.716, 222.93, 246.376, 272.287, 300.923, 332.572, 367.548, 406.204, 448.924, 496.138, 548.317, 605.984, 669.716, 740.15, 817.993, 904.021, 999.098, 1104.17, 1220.3, 1348.64, 1490.48, 1647.23, 1820.48, 2011.94, 2223.53, 2457.38, 2715.83, 3001.46, 3317.12, 3665.99, 4051.54, 4477.65, 4948.56, 5469.01, 6044.19, 6679.86, 7382.39, 8158.8, 9016.87, 9965.19, 11013.2 };
      double[] tanh = { 0, 0.099668, 0.17375, 0.2911, 0.379949, 0.46217, 0.53705, 0.604368, 0.664037, 0.71298, 0.76194, 0.800499, 0.833655, 0.86123, 0.885352, 0.90518, 0.92169, 0.935409, 0.946806, 0.956237, 0.964028, 0.970452, 0.975743, 0.980096, 0.983675, 0.98661, 0.989027, 0.99107, 0.992632, 0.993963, 0.995055, 0.995949, 0.996682, 0.997283, 0.997775, 0.99818, 0.998508, 0.998778, 0.999, 0.99911, 0.999329, 0.999451, 0.99955, 0.999632, 0.999699, 0.999753, 0.999798, 0.999835, 0.999865, 0.999889, 0.999909, 0.999926, 0.999939, 0.99995, 0.999959, 0.999967, 0.999973, 0.999978, 0.999982, 0.999985, 0.999988, 0.99999, 0.999992, 0.999993, 0.999994, 0.999995, 0.999996, 0.999997, 0.999998, 0.999998, 0.999998, 0.999999, 0.999999, 0.999999, 0.999999, 0.999999, 0.999999, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
      double[] sinh = { 0, 0.100167, 0.201336, 0.30452, 0.410752, 0.521095, 0.636654, 0.758584, 0.888106, 1.02652, 1.1752, 1.33565, 1.50946, 1.69838, 1.9043, 2.12928, 2.37557, 2.64563, 2.94217, 3.26816, 3.62686, 4.02186, 4.45711, 4.93696, 5.46623, 6.0502, 6.69473, 7.40626, 8.19192, 9.05956, 10.0179, 11.0765, 12.2459, 13.5379, 14.9654, 16.5426, 18.2855, 20.2113, 22.3394, 24.6911, 27.2899, 30.1619, 33.3357, 36.8431, 40.7193, 45.003, 49.7371, 54.969, 60.7511, 67.1412, 74.2032, 82.0079, 90.6334, 100.166, 110.701, 122.344, 135.211, 149.432, 165.148, 182.517, 201.713, 222.928, 246.374, 272.285, 300.922, 332.57, 367.547, 406.202, 448.923, 496.137, 548.316, 605.983, 669.715, 740.15, 817.992, 904.021, 999.098, 1104.17, 1220.3, 1348.64, 1490.48, 1647.23, 1820.48, 2011.94, 2223.53, 2457.38, 2715.83, 3001.46, 3317.12, 3665.99, 4051.54, 4477.65, 4948.56, 5469.01, 6044.19, 6679.86, 7382.39, 8158.8, 9016.87, 9965.19, 11013.2 };
      double[] tan = { 0, 0.100335, 0.20271, 0.309336, 0.422793, 0.546302, 0.684137, 0.842288, 1.02964, 1.26016, 1.55741, 1.96476, 2.57215, 3.6021, 5.79788, 14.1014, -34.2325, -7.6966, -4.28626, -2.9271, -2.18504, -1.70985, -1.37382, -1.11921, -0.916014, -0.747022, -0.601597, -0.472728, -0.35553, -0.246405, -0.142547, -0.0416167, 0.0584739, 0.159746, 0.264317, 0.374586, 0.493467, 0.624733, 0.773556, 0.947425, 1.15782, 1.42353, 1.77778, 2.28585, 3.09632, 4.63733, 8.86017, 80.7128, -11.3849, -5.26749, -3.38052, -2.44939, -1.88564, -1.50127, -1.21754, -0.995584, -0.813943, -0.659731, -0.524666, -0.403111, -0.291006, -0.185262, -0.0833777, 0.0168163, 0.117349, 0.220277, 0.327858, 0.442757, 0.56834, 0.709111, 0.871448, 1.06489, 1.30462, 1.61656, 2.04928, 2.70601, 3.85227, 6.44287, 18.5068, -21.7151, -6.79971, -3.9824, -2.77375, -2.09138, -1.64571, -1.32636, -1.08203, -0.885557, -0.721147, -0.578924, -0.452316, -0.336701, -0.228642, -0.12543, -0.024783, 0.0753642, 0.177038, 0.282388, 0.393883, 0.514553, 0.648361 };
      double[] doubles = { 1.5, 2.6, 3.7, 4.8, 5.9, 8.0, 20.0 };
      double[] pol = { -5, -4.89, -4.76, -4.61, -4.44, -4.25, -4.04, -3.81, -3.56, -3.29, -3, -2.69, -2.36, -2.01, -1.64, -1.25, -0.84, -0.41, 0.04, 0.51, 1, 1.51, 2.04, 2.59, 3.16, 3.75, 4.36, 4.99, 5.64, 6.31, 7, 7.71, 8.44, 9.19, 9.96, 10.75, 11.56, 12.39, 13.24, 14.11, 15, 15.91, 16.84, 17.79, 18.76, 19.75, 20.76, 21.79, 22.84, 23.91, 25, 26.11, 27.24, 28.39, 29.56, 30.75, 31.96, 33.19, 34.44, 35.71, 37, 38.31, 39.64, 40.99, 42.36, 43.75, 45.16, 46.59, 48.04, 49.51, 51, 52.51, 54.04, 55.59, 57.16, 58.75, 60.36, 61.99, 63.64, 65.31, 67, 68.71, 70.44, 72.19, 73.96, 75.75, 77.56, 79.39, 81.24, 83.11, 85, 86.91, 88.84, 90.79, 92.76, 94.75, 96.76, 98.79, 100.84, 102.91, 105 };

      List<PlotData> pdSimulation = new List<PlotData>
      {
        new PlotData("Cos", cos),
        new PlotData("Sin", sin),
        new PlotData("Cosh", cosh),
        new PlotData("Tanh", tanh)
      };
      List<PlotData> pdForecast = new List<PlotData>
      {
        new PlotData("Sinh", sinh),
        new PlotData("Tan", tan),
        new PlotData("Doubles", doubles),
        new PlotData("x^2+x-5", pol)
      };

      pvdsSimulation = PlotBuilder(pdSimulation, PlotType.Simulation);
      pvdsForecast = PlotBuilder(pdForecast, PlotType.Forecast);


      StatisticsTableData std11 = new StatisticsTableData(defaultOptions.sampleSize, 2, 3, 4, 5, 6, 7, defaultOptions.startDate, 99);
      StatisticsTableData std12 = new StatisticsTableData(defaultOptions.sampleSize, 2, 3, 4, 5, 6, 7, defaultOptions.startDate, 99);
      StatisticsTableData std13 = new StatisticsTableData(defaultOptions.sampleSize, 2, 3, 4, 5, 6, 7, defaultOptions.startDate, 99);
      StatisticsTableData std14 = new StatisticsTableData(defaultOptions.sampleSize, 2, 3, 4, 5, 6, 7, defaultOptions.startDate, 99);
      StatisticsTableData std21 = new StatisticsTableData(defaultOptions.sampleSize, 9, 8, 7, 6, 5, 4, defaultOptions.startDate, 99);
      StatisticsTableData std22 = new StatisticsTableData(defaultOptions.sampleSize, 9, 8, 7, 6, 5, 4, defaultOptions.startDate, 99);
      StatisticsTableData std23 = new StatisticsTableData(defaultOptions.sampleSize, 9, 8, 7, 6, 5, 4, defaultOptions.startDate, 99);
      StatisticsTableData std24 = new StatisticsTableData(defaultOptions.sampleSize, 9, 8, 7, 6, 5, 4, defaultOptions.startDate, 99);
      stdSimulation = new List<StatisticsTableData>
      {
        std11,
        std12,
        std13,
        std14
      };
      stdForecast = new List<StatisticsTableData>
      {
        std21,
        std22,
        std23,
        std24
      };

      stbSimulation = StatisticalTableBuilder(PlotType.Simulation);
      stbForecast = StatisticalTableBuilder(PlotType.Forecast);
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

      html += PngToPdf(PlotType.Simulation);
      html += PngToPdf(PlotType.Forecast);

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
    private static string PngToPdf(PlotType plotType)
    {
      PngExporter pngExporter = new PngExporter { Width = 600, Height = 400, Background = OxyColors.White };
      string html = "";
      int index = 0;
      string fileName = "plot_";
      List<(PlotViewData, StatisticsTableData)> zipList = new List<(PlotViewData, StatisticsTableData)>();

      switch (plotType)
      {
        case PlotType.Simulation:
          fileName += "simulation_";
          zipList = pvdsSimulation.Zip(stdSimulation, (p, s) => (p, s)).ToList();
          break;
        case PlotType.Forecast: fileName += "forecast_";
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
