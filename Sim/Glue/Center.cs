using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Forecast.Error;
using Forecast.Method.AverageBased;
using Forecast.Method.LinearRegression;
using Simulador;
using Utils;

namespace Glue
{
  public class Center
  {
    private static (int, string) currentProduct = (-1, null);
    private static List<(DateTime, double)> currentProductData = new List<(DateTime, double)>();
    private static List<(DateTime, double)> currentProductDataDaily = new List<(DateTime, double)>();
    private static double[] rawDouble;
    private static List<((double[], double[]), string)> forecasts = new List<((double[], double[]), string)>();

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
    public static List<InventoryOutput> SimulationData((int, string) product)
    {
      GetProductData(product);
      List<(Distributions, Distributions)> distributions = new List<(Distributions, Distributions)>{
        (Distributions.Poisson, Distributions.Exponential),
        (Distributions.Normal, Distributions.Normal),
        (Distributions.Exponential, Distributions.Poisson),
        (Distributions.UniformCont, Distributions.Exponential),
        (Distributions.UniformDisc, Distributions.Normal)
      };
      List<InventoryOutput> simulations = new List<InventoryOutput>();
      int endOfSimulation = CalculateEndOfSimulation();
      foreach ((Distributions, Distributions) d in distributions)
      {
        simulations.Add(Inventory.Simulation(0, endOfSimulation, double.MaxValue, currentProductData, d.Item1, d.Item2, period));
      }
      return simulations;
    }
    public static List<((double[], double[]), string)> ForecastData((int, string) product)
    {
      GetProductData(product);
      List<double> listDoubles = new List<double>();
      foreach ((DateTime, double) dd in currentProductData) { listDoubles.Add(dd.Item2); }
      rawDouble = listDoubles.ToArray();
      forecasts = new List<((double[], double[]), string)>()
      {
        (SimpleLinearRegression.Calculate(rawDouble, 1), SimpleLinearRegression.Name),
        (SimpleAverage.Calculate(rawDouble, 1), SimpleAverage.Name),
        (MovingAverage.Calculate(rawDouble, 3), MovingAverage.Name),
        (DoubleMovingAverage.Calculate(rawDouble, 1, 3), DoubleMovingAverage.Name),
        (SimpleExponentialSmoothing.Calculate(rawDouble, 0.2), SimpleExponentialSmoothing.Name),
        (Holt.CalculateBest(rawDouble, 1), Holt.Name),
        (Winters.CalculateBest(rawDouble, 1, (int)period), Winters.Name)
      };

      return forecasts;
    }
    public static List<ForecastStatisticsTableData> ForecastStatData()
    {
      List<ForecastStatisticsTableData> stds = new List<ForecastStatisticsTableData>();

      foreach (((double[], double[]), string) f in forecasts)
      {
        double mad = MeanAbsoluteDeviation.Calculation(rawDouble, f.Item1.Item1);
        double mape = MeanAbsolutePercentageError.Calculation(rawDouble, f.Item1.Item1);
        double mpe = MeanPercentageError.Calculation(rawDouble, f.Item1.Item1);
        double mse = MeanSquaredError.Calculation(rawDouble, f.Item1.Item1);
        double rmsd = RootMeanSquareDeviation.Calculation(rawDouble, f.Item1.Item1);
        ForecastStatisticsTableData std = new ForecastStatisticsTableData(
          0, 1, mad, mape, mpe, mse, rmsd, currentProductData.First().Item1, f.Item1.Item2.Last());
        stds.Add(std);
      }

      return stds;
    }
    private static int CalculateEndOfSimulation()
    {
      DateTime firstDate = currentProductData.First().Item1;
      DateTime lastDate = currentProductData.Last().Item1;
      int endOfSimulation = (int)lastDate.Subtract(firstDate).TotalSeconds;
      switch (period)
      {
        case Period.Diario: endOfSimulation += 86401; break;
        case Period.Mensual: endOfSimulation += 2678401; break;
        case Period.Anual: endOfSimulation += 980294401; break;
        default: break;
      }
      return endOfSimulation;
    }
  }
}
