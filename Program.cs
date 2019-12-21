using NUnit.Framework;
using System;
using System.Diagnostics;
using System.Linq;

/*
 * Problem posted here: https://www.codewars.com/kata/conways-game-of-life-unlimited-edition/train/csharp
 *
 * The rules of the game are:
 *
 * Any live cell with fewer than two live neighbours dies, as if caused by underpopulation.
 * Any live cell with more than three live neighbours dies, as if by overcrowding.
 * Any live cell with two or three live neighbours lives on to the next generation.
 * Any dead cell with exactly three live neighbours becomes a live cell.
 *
 * Each cell's neighborhood is the 8 cells immediately around it (i.e. Moore Neighborhood). The universe is infinite in both the x and y dimensions and all cells are initially dead - except for those specified in the arguments.
 */
public class ConwayLife
{
  private static int GetAt(int[,] cells, int x, int y)
  {
      if (x < 0 || y < 0 || x >= cells.GetLength(0) || y >= cells.GetLength(1))
      {
          return 0;
      }
      else
      {
          return cells[x, y];
      }
  }

  private static int CountNeighbors(int[,] cells, int x, int y)
  {
      return Enumerable.Range(x - 1, 3)
                       .Select(_x => Enumerable.Range(y - 1, 3)
                                               .Select(_y => GetAt(cells, _x, _y)).Sum()).Sum() - cells[x, y];
    }

  private static int[,] CopyGeneration(int[,] cells)
  {
      return (int[,]) cells.Clone();
  }

  private static void SwitchCellState(int[,] cells, int x, int y, int state)
  {
      cells[x, y] = state;
  }

  public static int[,] GetGeneration(int[,] cells, int generation)
  {
      for (int i = 0; i < generation; i++)
      {
          var nextGeneration = CopyGeneration(cells);
          for (int xi = 0; xi < cells.GetLength(0); xi++)
          {
              for (int yi = 0; yi < cells.GetLength(1); yi++)
              {
                  int neighbors = CountNeighbors(cells, xi, yi);
                  if (neighbors < 2)
                  {
                      SwitchCellState(nextGeneration, xi, yi, 0);
                  }
                  else if (neighbors > 3)
                  {
                      SwitchCellState(nextGeneration, xi, yi, 0);
                  }
                  else if (neighbors == 3 && cells[xi, yi] == 0)
                  {
                      SwitchCellState(nextGeneration, xi, yi, 1);
                  }
              }
          }
          cells = CopyGeneration(nextGeneration);
      }
      return cells;
 }

 public static void PrintGeneration(int[,] cells)
 {
    for (int x = 0; x < cells.GetLength(0); x++)
    {
        for (int y = 0; y < cells.GetLength(1); y++)
        {
            Console.Write($"{cells[x, y]}");
        }
        Console.WriteLine();
    }
 }

 public static void Main(string[] argv)
 {
     Debug.Assert(argv.GetLength(0) == 1);

     int[][,] gliders =
     {
         new int[,] {{1,0,0},{0,1,1},{1,1,0}},
     };

     Console.WriteLine("Glider");
     PrintGeneration(gliders[0]);
     Console.WriteLine();

     Enumerable.Range(0, Int32.Parse(argv[0])).ToList().ForEach(i => {
        Console.WriteLine($"Generation: {i}");
        PrintGeneration(ConwayLife.GetGeneration(gliders[0], i));
        Console.WriteLine();
     });
 }
}
