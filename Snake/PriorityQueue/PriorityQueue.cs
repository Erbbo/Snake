using System.Collections.Generic;
using Snake.Algorithm;
using Snake.Interfaces;

namespace Snake.PriorityQueue
{
  public class PriorityQueue : IHasNeighbors<Node>
  {
    public IList<Node> Node { get; private set; }

    public IEnumerable<Node> HasNeighbors
    {
      get
      {
        return Node;
      }
      private set
      {
        Node = CreateNode();
      }
    }

    public PriorityQueue(PriorityQueue queue) { }

    /// <summary>
    /// Calculate the Point that was passed based on a node
    /// </summary>
    /// <returns></returns>
    public List<Node> CreateNode()
    {
      return new List<Node>();
    }
  }
}
