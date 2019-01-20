using Gtk;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GtkOxyPlot.GTK
{
  class Product
  {
    public static Window mainWindow = null;
    private static string activeElement;

    public static void Init()
    {
      if (null == mainWindow) mainWindow = new Window("Products");
      mainWindow.Destroyed += new EventHandler(delegate (object o, EventArgs args) { Application.Quit(); });
      mainWindow.SetDefaultSize(300, 100);

      ComboBox comboBox = ComboBox.NewText();
      comboBox.Changed += new EventHandler(OnComboBoxChanged);
      Products().ForEach(p => comboBox.AppendText(p));

      Button button = new Button("Aceptar");
      button.Clicked += new EventHandler(delegate (object o, EventArgs args) { RunMainWindow(); });

      Table table = new Table(3, 1, true);
      table.Attach(new Label("Seleccione un producto"), 0, 1, 0, 1);
      table.Attach(comboBox, 0, 1, 1, 2);
      table.Attach(button, 0, 1, 2, 3);

      mainWindow.Add(table);
      mainWindow.ShowAll();
    }
    public static List<string> Products()
    {
      List<string> ps = new List<string> { "Pera", "Manzana", "Banana" };
      return ps;
    }
    public static void RunMainWindow()
    {
      if (null == activeElement) return;
      mainWindow.HideAll();
      DefaultOptions defaultOptions = new DefaultOptions();
      (List<PlotViewData>, List<PlotViewData>, List<TableData>, List<TableData>) data = Helper.GatherData(defaultOptions);
      Helper.InitWindow(data.Item1, data.Item2, data.Item3, data.Item4);
    }
    private static void OnComboBoxChanged (object o, EventArgs args)
    {
      ComboBox combo = o as ComboBox;
      if (o == null) return;
      if (combo.GetActiveIter(out TreeIter iter)) activeElement = (string)combo.Model.GetValue(iter, 0);
    }
  }
}
