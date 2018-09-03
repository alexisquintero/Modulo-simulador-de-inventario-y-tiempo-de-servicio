namespace Simulador.Utils
{
  class LinkedList<A>
  {
    public LinkedList()
    {
      head = new Node<A>();
    }
    public LinkedList(Node<A> pNode)
    {
      head = pNode;
    }
    private Node<A> head;
    public A Head()
    {
      return head.data;
    }
    public LinkedList<A> Tail()
    {
      if (head.next.IsEmpty())
      {
        return new LinkedList<A>();
      }
      else
      {
        return new LinkedList<A>(head.next);
      }
    }
    public bool IsEmpty()
    {
      return head.IsEmpty();
    }
    public void Add(A data)
    {
      head.Add(data);
    }
  }
}
