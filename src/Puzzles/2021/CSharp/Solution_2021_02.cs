using AdventOfCode.Utils;

namespace AdventOfCode.Solutions
{
  public class Solution_2021_02 : CSharpSolution
  {
    public override void Solve(PuzzleInput input)
    {
      var lines = input.GetLines();

      int x = 0;
      int y = 0;

      foreach (string s in lines)
      {
        var parts = s.Split(" ");
        int delta = int.Parse(parts[1]);

        if (parts[0].Equals("forward"))
        {
          x += delta;
        }
        else if (parts[0].Equals("up"))
        {
          y -= delta;
        }
        else if (parts[0].Equals("down"))
        {
          y += delta;
        }
      }

      SubmitPartOne(x * y);

      x = 0;
      y = 0;

      int aim = 0;
      foreach (string s in lines)
      {
        var parts = s.Split(" ");
        int delta = int.Parse(parts[1]);

        if (parts[0].Equals("forward"))
        {
          x += delta;
          y += delta * aim;
        }
        else if (parts[0].Equals("up"))
        {
          aim -= delta;
        }
        else if (parts[0].Equals("down"))
        {
          aim += delta;
        }
      }

      SubmitPartTwo(x * y);
    }
  }
}