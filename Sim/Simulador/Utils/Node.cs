using System;

namespace Simulador.Utils
{
  public class Node<A>
  {
    public Node<A> next;
    public A data;
    public void Add(A pData)
    {
     if (data == null) data = pData;
     else next.Add(pData);
    }
    public bool IsEmpty()
    {
      return data == null;
    }
  }
}
