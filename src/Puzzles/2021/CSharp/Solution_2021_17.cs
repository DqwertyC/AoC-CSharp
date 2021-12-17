using AdventOfCode.Utils;
using System.Collections.Generic;
using System;

namespace AdventOfCode.Solutions
{
  public class Solution_2021_17 : CSharpSolution
  {
    public override void Solve(PuzzleInput input)
    {
      char[] splitChars = { '.', '=', ',', '\n' };
      var parts = input.rawInput.Split(splitChars, System.StringSplitOptions.RemoveEmptyEntries);

      (int x, int y) minPos = (int.Parse(parts[1]), int.Parse(parts[4]));
      (int x, int y) maxPos = (int.Parse(parts[2]), int.Parse(parts[5]));

      (int x, int y) minVel = ((int)Math.Sqrt(minPos.x * 2), minPos.y);
      (int x, int y) maxVel = (maxPos.x, (-minPos.y) - 1);

      SubmitPartOne((maxVel.y * (maxVel.y + 1)) / 2);

      int sum = 0;

      for (int velx = minVel.x; velx <= maxVel.x; velx++)
      {
        for (int vely = minVel.y; vely <= maxVel.y; vely++)
        {
          bool inZone = false;
          (int x, int y) vel = (velx, vely);
          (int x, int y) pos = (0, 0);

          while (pos.x <= maxPos.x && pos.y >= minPos.y && !inZone)
          {
            pos.x += vel.x;
            pos.y += vel.y;

            vel.x -= Math.Sign(vel.x);
            vel.y -= 1;

            if (pos.x >= minPos.x && pos.x <= maxPos.x && pos.y >= minPos.y && pos.y <= maxPos.y)
            {
              inZone = true;
            }

            sum += inZone ? 1 : 0;
          }
        }
      }

      SubmitPartTwo(sum);
    }
  }
}