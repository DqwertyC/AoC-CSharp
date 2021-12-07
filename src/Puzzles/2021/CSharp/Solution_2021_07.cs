using AdventOfCode.Utils;
using System.Collections.Generic;
using System;

namespace AdventOfCode.Solutions
{
  public class Solution_2021_07 : CSharpSolution
  {
    PuzzleInput sample = new PuzzleInput("16,1,2,0,4,2,7,1,2,14");

    public override void Solve(PuzzleInput input)
    {
      List<int> values = input.GetInts(",");
      values.Sort();

      int median = values[values.Count / 2];

      SubmitPartOne(CalculateLinearFuel(values, median));

      long sum = 0;
      foreach (int i in values)
      {
        sum += i;
      }

      int average = (int)(sum / values.Count);

      long triangleFuel = CalculateTriangleFuel(values, average);
      triangleFuel = Math.Min(triangleFuel, CalculateTriangleFuel(values, average + 1));
      triangleFuel = Math.Min(triangleFuel, CalculateTriangleFuel(values, average - 1));

      SubmitPartTwo(triangleFuel);
    }

    public int CalculateLinearFuel(List<int> startVals, int endVal)
    {
      int sum = 0;
      foreach (int i in startVals)
      {
        sum += Math.Abs(endVal - i);
      }

      return sum;
    }

    public long CalculateTriangleFuel(List<int> startVals, int endVal)
    {
      long sum = 0;
      foreach (int i in startVals)
      {
        int absDist = Math.Abs(endVal - i);
        sum += (absDist * (absDist + 1)) / 2;
      }

      return sum;
    }
  }
}