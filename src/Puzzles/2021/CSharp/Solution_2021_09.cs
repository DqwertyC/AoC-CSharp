using AdventOfCode.Utils;
using System.Collections.Generic;

namespace AdventOfCode.Solutions
{
  public class Solution_2021_09 : CSharpSolution
  {
    public static (int, int)[] neighbors = { (1, 0), (0, 1), (-1, 0), (0, -1) };

    string test = @"2199943210
3987894921
9856789892
8767896789
9899965678";

    public override void Solve(PuzzleInput input)
    {
      PuzzleInput testInput = new PuzzleInput(test);
      var map = input.GetDictionary();
      List<(int, int)> lowPoints = new List<(int, int)>();

      foreach ((int x, int y) key in map.Keys)
      {
        bool isLowPoint = true;
        foreach ((int x, int y) dir in neighbors)
        {
          (int x, int y) search = (key.x + dir.x, key.y + dir.y);

          if (map.ContainsKey(search) && map[search] <= map[key])
          {
            isLowPoint = false;
          }
        }

        if (isLowPoint)
        {
          lowPoints.Add(key);
        }
      }

      int sum = lowPoints.Count;
      foreach ((int, int) key in lowPoints)
      {
        sum += (map[key] - '0');
      }

      SubmitPartOne(sum);

      List<int> basinSizes = new List<int>();

      foreach ((int, int) key in lowPoints)
      {
        HashSet<(int, int)> inBasin = new HashSet<(int, int)>();
        Queue<(int, int)> toVisit = new Queue<(int, int)>();
        inBasin.Add(key);
        toVisit.Enqueue(key);

        while (toVisit.Count > 0)
        {
          (int x, int y) current = toVisit.Dequeue();

          foreach ((int x, int y) dir in neighbors)
          {
            (int, int) search = (current.x + dir.x, current.y + dir.y);

            if (!inBasin.Contains(search) && map.ContainsKey(search) && map[search] != '9')
            {
              inBasin.Add(search);
              toVisit.Enqueue(search);
            }
          }
        }

        basinSizes.Add(inBasin.Count);
      }

      basinSizes.Sort();

      long product = 1;

      for (int i = 1; i <= 3; i++)
      {
        product *= basinSizes[basinSizes.Count - i];
      }

      SubmitPartTwo(product);
    }
  }
}