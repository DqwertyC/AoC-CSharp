using AdventOfCode.Utils;
using System.Collections.Generic;

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
      // Write your puzzle solution here!

      SubmitPartOne(string.Empty);
      SubmitPartTwo(string.Empty);
    }
  }
}