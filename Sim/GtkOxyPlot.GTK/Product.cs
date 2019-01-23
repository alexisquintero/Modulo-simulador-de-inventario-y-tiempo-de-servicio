using Glue;
using Gtk;
using System;
using System.Collections.Generic;
using Utils;

namespace GtkOxyPlot.GTK
{
  class Product
  {
    public static Window mainWindow = null;
    public static (int, string) activeElement;
    private static List<(int, string)> products;

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
      List<string> ps = new List<string>();
      products = Center.StartData();
      foreach ((int, string) ip in products) { ps.Add(ip.Item2); }
      return ps;
    }
    public static void RunMainWindow()
    {
      if (null == activeElement.Item2) return;
      mainWindow.HideAll();
      DefaultOptions defaultOptions = new DefaultOptions();
      Helper.GatherData(defaultOptions);
      Helper.InitWindow();
    }
    private static void OnComboBoxChanged (object o, EventArgs args)
    {
      ComboBox combo = o as ComboBox;
      if (o == null) return;
      if (combo.GetActiveIter(out TreeIter iter))
      {
        string activeElementName = (string)combo.Model.GetValue(iter, 0);
        foreach ((int, string) ip in products)
        {
          activeElement = ip.Item2 == activeElementName ? ip : activeElement;
        }
      }
    }
  }
}
