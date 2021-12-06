using AdventOfCode.Utils;
using System.Collections.Generic;

namespace AdventOfCode.Solutions
{
  public class Solution_2021_05 : CSharpSolution
  {
    public override void Solve(PuzzleInput input)
    {
      var lines = input.GetLines();
      List<Region> diagRegions = new List<Region>();
      List<Region> flatRegions = new List<Region>();
      Dictionary<(int, int), int> counts = new Dictionary<(int, int), int>();
      HashSet<(int, int)> duplicates = new HashSet<(int, int)>();

      foreach (string line in lines)
      {
        Region region = new Region(line);

        if (region.IsLine())
        {
          flatRegions.Add(region);
        }
        else
        {
          diagRegions.Add(region);
        }
      }

      foreach (Region region in flatRegions)
      {
        var points = region.GetPoints();
        foreach ((int x, int y) point in points)
        {
          if (!counts.ContainsKey(point))
          {
            counts[point] = 0;
          }

          counts[point] = counts[point] + 1;

          if (counts[point] == 2)
          {
            duplicates.Add(point);
          }
        }
      }

      SubmitPartOne(duplicates.Count);

      foreach (Region region in diagRegions)
      {
        foreach ((int x, int y) point in region.GetPoints())
        {
          if (!counts.ContainsKey(point))
          {
            counts[point] = 0;
          }

          counts[point] = counts[point] + 1;

          if (counts[point] == 2)
          {
            duplicates.Add(point);
          }
        }
      }

      SubmitPartTwo(duplicates.Count);
    }

    private class Region
    {
      public int x0;
      public int y0;
      public int x1;
      public int y1;

      public Region(string input)
      {
        string[] coords = input.Split(" -> ");
        string[] coordsL = coords[0].Split(",");
        string[] coordsR = coords[1].Split(",");

        x0 = int.Parse(coordsL[0]);
        y0 = int.Parse(coordsL[1]);
        x1 = int.Parse(coordsR[0]);
        y1 = int.Parse(coordsR[1]);
      }

      public List<(int, int)> GetPoints()
      {
        List<(int, int)> newPoints = new List<(int, int)>();

        int deltaX = x1 - x0;
        int deltaY = y1 - y0;

        deltaX = deltaX > 0 ? 1 : deltaX < 0 ? -1 : 0;
        deltaY = deltaY > 0 ? 1 : deltaY < 0 ? -1 : 0;

        int x = x0;
        int y = y0;

        while (x != x1 || y != y1)
        {
          newPoints.Add((x, y));
          x += deltaX;
          y += deltaY;
        }

        newPoints.Add((x1, y1));
        return newPoints;
      }

      public bool IsLine()
      {
        return ((x0 == x1) || (y0 == y1));
      }
    }
  }
}