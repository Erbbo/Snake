using System.Collections.Generic;

namespace Snake.Interfaces
{
  public interface IHasNeighbors<T> where T : new()
  {
    IEnumerable<T> HasNeighbors { get; }
  }
}
