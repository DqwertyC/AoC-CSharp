using AdventOfCode.Utils;
using System.Collections.Generic;

namespace AdventOfCode.Solutions
{
  public class Solution_2021_21 : CSharpSolution
  {
    public override void Solve(PuzzleInput input)
    {
      var lines = input.GetLines();

      int aStart = lines[0][lines[0].Length - 1] - '0';
      int bStart = lines[1][lines[1].Length - 1] - '0';

      int aScore = 0;
      int bScore = 0;

      int round = 0;

      while (aScore < 1000 && bScore < 1000)
      {
        round++;
        aScore += 1 + (aStart + 9 * round * round - 3 * round - 1) % 10;
        bScore += aScore < 1000 ? 1 + (bStart + 9 * (round * round + round) - 3 * round - 1) % 10 : 0;
      }

      int rolls = (aScore < 1000 ? 6 * round : 6 * round - 3);
      int score = rolls * (aScore < 1000 ? aScore : bScore);

      SubmitPartOne(score);

      Dictionary<int, int> rollCounts = new Dictionary<int, int>();
      rollCounts[3] = 1;
      rollCounts[4] = 3;
      rollCounts[5] = 6;
      rollCounts[6] = 7;
      rollCounts[7] = 6;
      rollCounts[8] = 3;
      rollCounts[9] = 1;

      Dictionary<(int aPos, int aScore, int bPos, int bScore), ulong> oldScoreCount = new Dictionary<(int, int, int, int), ulong>();
      oldScoreCount[(aStart, 0, bStart, 0)] = 1;

      bool isPlayerA = true;
      ulong aWins = 0;
      ulong bWins = 0;

      while (oldScoreCount.Count > 0)
      {
        Dictionary<(int aPos, int aScore, int bPos, int bScore), ulong> newScoreCount = new Dictionary<(int, int, int, int), ulong>();

        foreach (var key in oldScoreCount.Keys)
        {
          foreach (var roll in rollCounts.Keys)
          {
            if (isPlayerA)
            {
              int newPos = ((key.aPos + roll - 1) % 10) + 1;
              int newScore = key.aScore + newPos;
              ulong newCount = (ulong)oldScoreCount[key] * (ulong)rollCounts[roll];

              if (newScore < 21)
              {
                var newKey = (newPos, newScore, key.bPos, key.bScore);
                newScoreCount[newKey] = newScoreCount.GetValueOrDefault(newKey, 0UL) + (ulong)newCount;
              }
              else
              {
                aWins += newCount;
              }
            }
            else
            {
              int newPos = ((key.bPos + roll - 1) % 10) + 1;
              int newScore = key.bScore + newPos;
              ulong newCount = (ulong)oldScoreCount[key] * (ulong)rollCounts[roll];

              if (newScore < 21)
              {
                var newKey = (key.aPos, key.aScore, newPos, newScore);
                newScoreCount[newKey] = newScoreCount.GetValueOrDefault(newKey, 0UL) + newCount;
              }
              else
              {
                bWins += newCount;
              }
            }
          }
        }

        oldScoreCount = newScoreCount;
        isPlayerA = !isPlayerA;
      }

      SubmitPartTwo(aWins > bWins ? aWins : bWins);
    }
  }


}