using AdventOfCode.Utils;
using System.Collections.Generic;
using System;
using System.Text;

namespace AdventOfCode.Solutions
{
  public class Solution_2021_11 : CSharpSolution
  {

    string testInput = @"5483143223
2745854711
5264556173
6141336146
6357385478
4167524645
2176841721
6882881134
4846848554
5283751526";
    public override void Solve(PuzzleInput input)
    {
      PuzzleInput test = new PuzzleInput(testInput);
      Dictionary<(int, int), int> octoMap = input.GetDictionaryInt();
      (int, int)[] neighbors = { (-1, -1), (-1, 0), (-1, 1), (0, -1), (0, 1), (1, -1), (1, 0), (1, 1) };

      int flashCount = 0;

      HashSet<(int, int)> hasFlashed = new HashSet<(int, int)>();
      int stepCount = 0;

      while (hasFlashed.Count != 100)
      {
        stepCount++;
        hasFlashed = new HashSet<(int, int)>();
        Queue<(int, int)> willFlash = new Queue<(int, int)>();

        foreach ((int x, int y) octopus in octoMap.Keys)
        {
          octoMap[octopus] = octoMap[octopus] + 1;

          if (octoMap[octopus] >= 10 && !hasFlashed.Contains(octopus))
          {
            willFlash.Enqueue(octopus);
            hasFlashed.Add(octopus);

            while (willFlash.Count > 0)
            {
              flashCount++;
              (int x, int y) current = willFlash.Dequeue();
              foreach ((int x, int y) dir in neighbors)
              {
                (int, int) look = (current.x + dir.x, current.y + dir.y);
                if (octoMap.ContainsKey(look))
                {
                  octoMap[look] = octoMap[look] + 1;
                  if (octoMap[look] >= 10 && !hasFlashed.Contains(look))
                  {
                    willFlash.Enqueue(look);
                    hasFlashed.Add(look);
                  }
                }
              }
            }
          }
        }

        foreach ((int, int) octopus in hasFlashed)
        {
          octoMap[octopus] = 0;
        }

        if (stepCount == 100)
        {
          SubmitPartOne(flashCount);
        }
      }



      SubmitPartTwo(stepCount);
    }
  }
}