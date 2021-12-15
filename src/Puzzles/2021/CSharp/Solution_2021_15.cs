using AdventOfCode.Utils;
using System.Collections.Generic;

namespace AdventOfCode.Solutions
{
  public class Solution_2021_15 : CSharpSolution
  {

    string testString = @"1163751742
1381373672
2136511328
3694931569
7463417111
1319128137
1359912421
3125421639
1293138521
2311944581";

    (int, int)[] neighbors = { (1, 0), (0, 1), (-1, 0), (0, -1) };
    public override void Solve(PuzzleInput input)
    {
      PuzzleInput testInput = new PuzzleInput(testString);
      Dictionary<(int, int), int> riskLevelA = input.GetDictionaryInt();
      Dictionary<(int, int), int> riskLevelB = new Dictionary<(int, int), int>();

      (int x, int y) max = (0, 0);
      foreach ((int x, int y) key in riskLevelA.Keys)
      {
        max.x = key.x > max.x ? key.x : max.x;
        max.y = key.y > max.y ? key.y : max.y;
      }

      (int x, int y) size = (max.x + 1, max.y + 1);

      SubmitPartOne(GetMinRisk(riskLevelA, max));

      foreach ((int x, int y) key in riskLevelA.Keys)
      {
        for (int i = 0; i < 5; i++)
        {
          for (int j = 0; j < 5; j++)
          {
            int newRiskLevel = riskLevelA[key] + i + j;
            newRiskLevel = newRiskLevel <= 9 ? newRiskLevel : newRiskLevel - 9;
            riskLevelB[(size.x * i + key.x, size.y * j + key.y)] = newRiskLevel;
          }
        }
      }

      SubmitPartTwo(GetMinRisk(riskLevelB, (5 * size.x - 1, 5 * size.y - 1)));
    }

    public int GetMinRisk(Dictionary<(int, int), int> riskLevels, (int, int) target)
    {
      Dictionary<(int, int), int> minRiskForTile = new Dictionary<(int, int), int>();
      Queue<(int, int)> searchQueue = new Queue<(int, int)>();
      HashSet<(int, int)> inQueue = new HashSet<(int, int)>();

      minRiskForTile[(0, 0)] = 0;
      searchQueue.Enqueue((0, 0));
      inQueue.Add((0, 0));

      while (searchQueue.Count > 0)
      {
        (int x, int y) loc = searchQueue.Dequeue();
        inQueue.Remove(loc);

        foreach ((int x, int y) dir in neighbors)
        {
          (int x, int y) search = (loc.x + dir.x, loc.y + dir.y);

          if (riskLevels.ContainsKey(search))
          {
            int currentRisk = minRiskForTile.GetValueOrDefault(search, int.MaxValue);
            int newRisk = minRiskForTile[loc] + riskLevels[search];

            if (newRisk < currentRisk)
            {
              minRiskForTile[search] = newRisk;

              if (!inQueue.Contains(search))
              {
                searchQueue.Enqueue(search);
                inQueue.Add(search);
              }
            }
          }
        }
      }

      return minRiskForTile[target];
    }
  }
}