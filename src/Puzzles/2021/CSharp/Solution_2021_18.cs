using AdventOfCode.Utils;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Solutions
{
  public class Solution_2021_18 : CSharpSolution
  {
    string testString = @"[[[0,[4,5]],[0,0]],[[[4,5],[2,6]],[9,5]]]
[7,[[[3,7],[4,3]],[[6,3],[8,8]]]]
[[2,[[0,8],[3,4]]],[[[6,7],1],[7,[1,6]]]]
[[[[2,4],7],[6,[0,5]]],[[[6,8],[2,8]],[[2,1],[4,5]]]]
[7,[5,[[3,8],[1,4]]]]
[[2,[2,2]],[8,[8,1]]]
[2,9]
[1,[[[9,3],9],[[9,0],[0,7]]]]
[[[5,[7,4]],7],1]
[[[[4,2],2],6],[8,7]]";

    public override void Solve(PuzzleInput input)
    {
      PuzzleInput testInput = new PuzzleInput(testString);

      var lines = input.GetLines();
      PairElement root = new PairElement(lines[0]);

      for (int i = 1; i < lines.Count; i++)
      {
        root = root.Add(new PairElement(lines[i]));
      }

      SubmitPartOne(root.GetMagnitude());

      long maxSum = 0;

      foreach (string s1 in lines)
      {
        foreach (string s2 in lines)
        {
          if (s1 != s2)
          {
            PairElement p1 = new PairElement(s1);
            PairElement p2 = new PairElement(s2);
            PairElement p3 = new PairElement(p1, p2);
            long sum = p3.GetMagnitude();
            maxSum = sum > maxSum ? sum : maxSum;
          }
        }
      }


      SubmitPartTwo(maxSum);
    }

    private class PairElement
    {
      PairElement parent;
      PairElement left;
      PairElement right;
      bool isLiteral;
      int value = 0;
      int depth;

      public PairElement(PairElement l, PairElement r)
      {
        depth = 0;
        left = l;
        right = r;
        isLiteral = false;

        left.IncreaseDepth();
        right.IncreaseDepth();

        left.parent = this;
        right.parent = this;

        bool wasSimplified = false;

        do
        {
          wasSimplified = false;
          wasSimplified = TryExplode();
          if (!wasSimplified)
          {
            wasSimplified = TrySplit();
          }
        }
        while (wasSimplified);
      }

      public PairElement(int i, PairElement p)
      {
        value = i;
        isLiteral = true;
        parent = p;
        depth = parent == null ? 0 : parent.depth + 1;
      }

      public PairElement(string s, PairElement p = null)
      {
        parent = p;
        isLiteral = false;
        depth = parent == null ? 0 : parent.depth + 1;

        StringBuilder sbL = new StringBuilder();
        StringBuilder sbR = new StringBuilder();

        bool reachedMidpoint = false;
        int openCount = 0;

        for (int i = 1; i < s.Length - 1; i++)
        {
          if (reachedMidpoint)
          {
            sbR.Append(s[i]);
          }
          else
          {
            openCount += s[i] == '[' ? 1 : s[i] == ']' ? -1 : 0;

            if (openCount == 0 && s[i] == ',')
            {
              reachedMidpoint = true;
            }
            else
            {
              sbL.Append(s[i]);
            }
          }
        }

        if (!reachedMidpoint)
        {
          value = int.Parse(s.TrimEnd(']'));
          isLiteral = true;
        }
        else
        {
          left = new PairElement(sbL.ToString(), this);
          right = new PairElement(sbR.ToString(), this);
        }
      }

      private bool TryExplode()
      {
        if (left == null || right == null)
        {
          return false;
        }

        if (!left.isLiteral && left.TryExplode())
        {
          return true;
        }

        if (!right.isLiteral && right.TryExplode())
        {
          return true;
        }

        if (left.isLiteral && right.isLiteral && depth >= 4)
        {
          PairElement search;
          if (TryGetNearestLeft(out search))
          {
            search.value += left.value;
          }

          if (TryGetNearestRight(out search))
          {
            search.value += right.value;
          }

          left = null;
          right = null;
          value = 0;
          isLiteral = true;
          return true;
        }

        return false;
      }

      public long GetMagnitude()
      {
        if (isLiteral) return value;
        return (3 * left.GetMagnitude() + 2 * right.GetMagnitude());
      }

      public PairElement Add(PairElement o)
      {
        return new PairElement(this, o);
      }

      private void IncreaseDepth()
      {
        depth++;
        if (left != null) left.IncreaseDepth();
        if (right != null) right.IncreaseDepth();
      }

      private bool TrySplit()
      {
        if (isLiteral && value >= 10)
        {
          left = new PairElement(this.value / 2, this);
          right = new PairElement((this.value + 1) / 2, this);
          isLiteral = false;
          return true;
        }

        if (left != null && left.TrySplit())
        {
          return true;
        }

        if (right != null && right.TrySplit())
        {
          return true;
        }

        return false;
      }

      private bool TryGetNearestLeft(out PairElement result)
      {
        result = this;

        while (result.parent != null && result.parent.left == result)
        {
          result = result.parent;
        }

        if (result.parent == null) return false;
        result = result.parent.left;

        while (result.right != null)
        {
          result = result.right;
        }

        return true;
      }

      private bool TryGetNearestRight(out PairElement result)
      {
        result = this;

        while (result.parent != null && result.parent.right == result)
        {
          result = result.parent;
        }

        if (result.parent == null) return false;
        result = result.parent.right;

        while (result.left != null)
        {
          result = result.left;
        }

        return true;
      }

      public override string ToString()
      {
        if (isLiteral)
        {
          return "" + value;
        }
        else
        {
          return "[" + left.ToString() + "," + right.ToString() + "]";
        }
      }
    }
  }


}