using AdventOfCode.Utils;
using System.Collections.Generic;

namespace AdventOfCode.Solutions
{
  public class Solution_2021_01 : CSharpSolution
  {
    public override void Solve(PuzzleInput input)
    {
      var depths = input.GetInts("\n");
      List<int> summedDepths = new List<int>();
      int increaseCount = 0;

      for (int i = 1; i < depths.Count - 1; i++)
      {
        summedDepths.Add(depths[i - 1] + depths[i] + depths[i + 1]);
      }

      for (int i = 1; i < depths.Count; i++)
      {
        if (depths[i] > depths[i - 1])
        {
          increaseCount++;
        }
      }

      SubmitPartOne(increaseCount);

      increaseCount = 0;
      for (int i = 1; i < summedDepths.Count; i++)
      {
        if (summedDepths[i] > summedDepths[i - 1])
        {
          increaseCount++;
        }
      }

      SubmitPartTwo(increaseCount);
    }
  }
}