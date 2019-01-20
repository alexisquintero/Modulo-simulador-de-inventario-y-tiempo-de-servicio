using System;
using System.Collections.Generic;
using Gtk;

namespace GtkOxyPlot.GTK
{
  public class MainClass
  {
    [STAThread]
    public static void Main()
    {
      Application.Init();

      Product.Init();

      Application.Run();
    }
  }
}
