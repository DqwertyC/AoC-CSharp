using AdventOfCode.Utils;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Solutions
{
  public class Solution_2021_13 : CSharpSolution
  {

    string test = @"6,10
0,14
9,10
0,3
10,4
4,11
6,0
6,12
4,1
0,13
10,12
3,4
3,0
8,4
1,10
2,14
8,10
9,0

fold along y=7
fold along x=5";

    public override void Solve(PuzzleInput input)
    {
      PuzzleInput testInput = new PuzzleInput(test);

      List<List<string>> chunks = input.GetChunks();

      HashSet<(int, int)> points = new HashSet<(int, int)>();

      foreach (string line in chunks[0])
      {
        string[] coords = line.Split(",");
        points.Add((int.Parse(coords[0]), int.Parse(coords[1])));
      }

      bool firstFold = true;
      (int x, int y) bottomRight = (0, 0);
      foreach (string line in chunks[1])
      {
        string[] lineParts = line.Split("=");
        (int x, int y) max = (int.MaxValue, int.MaxValue);

        if (lineParts[0].EndsWith('x'))
        {
          max.x = int.Parse(lineParts[1]);
        }
        else
        {
          max.y = int.Parse(lineParts[1]);
        }

        bottomRight = (0, 0);
        HashSet<(int, int)> newPoints = new HashSet<(int, int)>();
        foreach ((int x, int y) coord in points)
        {
          (int x, int y) newCoord;
          newCoord.x = coord.x > max.x ? 2 * max.x - coord.x : coord.x;
          newCoord.y = coord.y > max.y ? 2 * max.y - coord.y : coord.y;
          newPoints.Add(newCoord);

          bottomRight.x = newCoord.x > bottomRight.x ? newCoord.x : bottomRight.x;
          bottomRight.y = newCoord.y > bottomRight.y ? newCoord.y : bottomRight.y;
        }

        points = newPoints;
        if (firstFold)
        {
          SubmitPartOne(points.Count);
          firstFold = false;
        }
      }

      StringBuilder sb = new StringBuilder();
      for (int y = 0; y <= bottomRight.y; y++)
      {
        for (int x = 0; x <= bottomRight.x; x++)
        {
          sb.Append(points.Contains((x, y)) ? "#" : " ");
        }
        sb.Append("\n");
      }

      SubmitPartTwo(sb.ToString());
    }
  }
}
