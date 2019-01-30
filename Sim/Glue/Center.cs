using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Forecast.Error;
using Forecast.Method.AverageBased;
using Forecast.Method.LinearRegression;
using Simulador;
using Utils;
using Utils.Normalized;

namespace Glue
{
  public class Center
  {
    private static (int, string) currentProduct = (-1, null);
    private static List<(DateTime, double)> currentProductData = new List<(DateTime, double)>();
    private static List<(DateTime, double)> currentProductDataDaily = new List<(DateTime, double)>();
    private static double[] rawDouble;
    private static List<(string, (double[], double[]))> forecasts = new List<(string, (double[], double[]))>();

    public static Period period;
    public static List<(int, string)> StartData()
    {
      return FromMicrosoftSQL.GetAllProducts();
      //Optionally
      //Read config file to determine source of info
      //Set connection string based on config file
      //Call appropriate data gathering class, MsSQL, Mongo, MySQL, ...
    }
    private static void GetProductData((int, string) product)
    {
      currentProduct = product;

      List<(DateTime, int)> rawData = new Func<List<(DateTime, int)>>(() =>
      {
        switch (period)
        {
          case Period.Diario: return FromMicrosoftSQL.GetProductSaleDataDaily(currentProduct.Item1, 200);
          case Period.Mensual: return FromMicrosoftSQL.GetProductSaleDataMonthly(currentProduct.Item1, 200);
          case Period.Anual: return FromMicrosoftSQL.GetProductSaleDataYearly(currentProduct.Item1, 200);
          default: return new List<(DateTime, int)>();
        }
      })();

      currentProductData = new List<(DateTime, double)>();
      foreach ((DateTime, int) da in rawData)
      {
        currentProductData.Add((da.Item1, da.Item2));
      }

      //Requiered for simulation
      List<(DateTime, int)> rawDataDaily = FromMicrosoftSQL.GetProductSaleDataDaily(currentProduct.Item1, 200);
      currentProductDataDaily = new List<(DateTime, double)>();
      foreach ((DateTime, int) da in rawDataDaily)
      {
        currentProductDataDaily.Add((da.Item1, da.Item2));
      }
    }
    public static List<(string, double[])> SimulationData((int, string) product)
    {
      GetProductData(product);
      Inventory.Initialization(0.0, 26272000.0, 99999.0, currentProductData, Distributions.Poisson, Distributions.Exponential);
      List<(DateTime, double)> simData = Inventory.Simulation();
      //groupby month
      List<(DateTime, double)> groupedData = simData.GroupBy(
        s => new DateTime(s.Item1.Year, s.Item1.Month, 1),
        s => s.Item2,
        (date, doub) => (date, doub.Sum())
      ).OrderBy(g => g.Item1).ToList();
      List<(DateTime, double)> zeroData = Zerolized.AddZeroValue(groupedData, period);
      List<double> onlyDoubles = new List<double>();
      foreach ((DateTime, double) z in zeroData) { onlyDoubles.Add(z.Item2); }
      List<(string, double[])> returnData = new List<(string, double[])>
      {
        ("Simulation", onlyDoubles.ToArray())
      };
      return returnData;
    }
    public static void SimulationStatData((int, string) product)
    {

    }
    public static List<(string, (double[], double[]))> ForecastData((int, string) product)
    {
      GetProductData(product);
      List<double> listDoubles = new List<double>();
      foreach ((DateTime, double) dd in currentProductData) { listDoubles.Add(dd.Item2); }
      rawDouble = listDoubles.ToArray();
      forecasts = new List<(string, (double[], double[]))>()
      {
        (SimpleLinearRegression.Name, SimpleLinearRegression.Calculate(rawDouble, 1)),
        (SimpleAverage.Name, SimpleAverage.Calculate(rawDouble, 1)),
        (MovingAverage.Name, MovingAverage.Calculate(rawDouble, 3)),
        (DoubleMovingAverage.Name, DoubleMovingAverage.Calculate(rawDouble, 1, 3)),
        (SimpleExponentialSmoothing.Name, SimpleExponentialSmoothing.Calculate(rawDouble, 0.2)),
        (Holt.Name, Holt.Calculate(rawDouble, 1, 0.2, 0.2)),
        (Winters.Name, Winters.Calculate(rawDouble, 0.2, 0.2, 0.2, 1))
      };

      return forecasts;
    }
    public static List<StatisticsTableData> ForecastStatData()
    {
      List<StatisticsTableData> stds = new List<StatisticsTableData>();

      foreach ((string, (double[], double[])) f in forecasts)
      {
        double mad = MeanAbsoluteDeviation.Calculation(rawDouble, f.Item2.Item1);
        double mape = MeanAbsolutePercentageError.Calculation(rawDouble, f.Item2.Item1);
        double mpe = MeanPercentageError.Calculation(rawDouble, f.Item2.Item1);
        double mse = MeanSquaredError.Calculation(rawDouble, f.Item2.Item1);
        double rmsd = RootMeanSquareDeviation.Calculation(rawDouble, f.Item2.Item1);
        StatisticsTableData std = new StatisticsTableData(
          0, 1, mad, mape, mpe, mse, rmsd, currentProductData.First().Item1, f.Item2.Item2.Last());
        stds.Add(std);
      }

      return stds;
    }
  }
}
