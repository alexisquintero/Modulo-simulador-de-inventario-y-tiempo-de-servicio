using System;
using Xunit;
using Data;
using Utils.Exceptions;

namespace UnitTest.DataTest
{
  public class BaseDataUnitTest
  {
    [Fact]
    public void GetAllProducts_Exception()
    {
      try
      {
        BaseData.GetAllProducts();
      }
      catch (BaseDataMethod e)
      {
        Assert.Equal(e.Message, BaseDataMethod.eMessage);
      }
    }
    [Fact]
    public void GetProductSaleData_Exception()
    {
      try
      {
        BaseData.GetProductSaleData(1, 2);
      }
      catch (BaseDataMethod e)
      {
        Assert.Equal(e.Message, BaseDataMethod.eMessage);
      }
    }
  }
}
