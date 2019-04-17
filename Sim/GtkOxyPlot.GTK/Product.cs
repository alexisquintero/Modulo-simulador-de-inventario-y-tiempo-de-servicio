using Glue;
using Gtk;
using System;
using System.Collections.Generic;
using Utils;
using System.Linq;

namespace GtkOxyPlot.GTK
{
  class Product
  {
    public static Window mainWindow = null;
    public static (int, string) activeElement;
    public static Period period;
    private static List<(int, string)> products;

    public static void Init()
    {
      if (null == mainWindow) mainWindow = new Window("Products");
      mainWindow.Destroyed += new EventHandler(delegate (object o, EventArgs args) { Application.Quit(); });
      mainWindow.SetDefaultSize(300, 100);

      ComboBox cbProducts = ComboBox.NewText();
      cbProducts.Changed += new EventHandler(OncbProductsChanged);
      Products().ForEach(p => cbProducts.AppendText(p));

      ComboBox cbPeriod = ComboBox.NewText();
      cbPeriod.Changed += new EventHandler(OncbPeriodChanged);
      foreach(Period p in Enum.GetValues(typeof(Period))) { cbPeriod.AppendText(p.ToString()); }
      cbPeriod.Active = 1;

      Button button = new Button("Aceptar");
      button.Clicked += new EventHandler(delegate (object o, EventArgs args) { RunMainWindow(); });

      Table table = new Table(4, 1, true);
      table.Attach(new Label("Seleccione un producto"), 0, 1, 0, 1);
      table.Attach(cbProducts, 0, 1, 1, 2);
      table.Attach(cbPeriod, 0, 1, 2, 3);
      table.Attach(button, 0, 1, 3, 4);

      mainWindow.Add(table);
      mainWindow.ShowAll();
    }
    public static List<string> Products()
    {
      List<string> ps = new List<string>();
      products = Center.StartData();
      (int, string) everything = (0, "Todos los productos");
      products.Insert(0, everything);
      foreach ((int, string) ip in products) { ps.Add(ip.Item2); }
      return ps;
    }
    public static void RunMainWindow()
    {
      if (null == activeElement.Item2) return;
      mainWindow.HideAll();
      DefaultOptions defaultOptions = new DefaultOptions();
      if (activeElement.Item1 == 0)
      {
        foreach ((int, string) product in products.Skip(1))
        {
          activeElement = product;
          Helper.GatherData(defaultOptions);
          Helper.Report(false);
          Init();
        }
        Helper.WriteReportToDisk(false);
      } else {
        Helper.GatherData(defaultOptions);
        Helper.InitWindow();
      }
    }
    private static void OncbProductsChanged (object o, EventArgs args)
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
    private static void OncbPeriodChanged (object o, EventArgs args)
    {
      ComboBox combo = o as ComboBox;
      if (o == null) return;
      if (combo.GetActiveIter(out TreeIter iter))
      {
        string strPeriod = (string)combo.Model.GetValue(iter, 0);
        Enum.TryParse(strPeriod, out Period tempPeriod);
        period = tempPeriod;
      }
    }
  }
}
