using AdventOfCode.Utils;
using System.Collections.Generic;

namespace AdventOfCode.Solutions
{
  public class Solution_2021_06 : CSharpSolution
  {

    PuzzleInput test = new PuzzleInput("3,4,3,1,2");

    public override void Solve(PuzzleInput input)
    {
      List<int> initial = input.GetInts(",");

      ulong[] counts = new ulong[9];
      for (int i = 0; i < counts.Length; i++)
      {
        counts[i] = 0;
      }

      foreach (int i in initial)
      {
        counts[i] = counts[i] + 1;
      }

      for (int i = 0; i < 256; i++)
      {
        if (i == 80)
        {
          ulong partOne = 0;
          foreach (ulong subCount in counts)
          {
            partOne += subCount;
          }
          SubmitPartOne(partOne);
        }

        ulong newFish = counts[0];
        for (ulong age = 0; age < 8; age++)
        {
          counts[age] = counts[age + 1];
        }

        counts[8] = newFish;
        counts[6] = counts[6] + newFish;
      }

      ulong partTwo = 0;
      foreach (ulong subCount in counts)
      {
        partTwo += subCount;
      }

      SubmitPartTwo(partTwo);
    }
  }
}