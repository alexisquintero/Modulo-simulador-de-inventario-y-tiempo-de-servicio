using System;
using System.Collections.Generic;
using Utils.Exceptions;

namespace Data
{
  public abstract class BaseData
  {
    public static List<(int, string)> GetAllProducts() { throw new BaseDataMethod(); }
    public static List<(DateTime, int)> GetProductSaleData(int productId, int sampleSize) { throw new BaseDataMethod(); }
  }
}
