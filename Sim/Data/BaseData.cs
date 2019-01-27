using System;
using System.Collections.Generic;
using System.Linq;
using Utils.Exceptions;

namespace Data
{
  public abstract class BaseData
  {
    public static List<(int, string)> GetAllProducts() { throw new BaseDataMethod(); }
    public static List<(DateTime, int)> GetProductSaleDataDaily(int productId, int sampleSize) { throw new BaseDataMethod(); }
    public static List<(DateTime, int)> GetProductSaleDataMonthly(int productId, int sampleSize) { throw new BaseDataMethod(); }
    public static List<(DateTime, int)> GetProductSaleDataYearly(int productId, int sampleSize) { throw new BaseDataMethod(); }
  }
}
