using AdventOfCode.Utils;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Solutions
{
  public class Solution_2021_03 : CSharpSolution
  {
    public override void Solve(PuzzleInput input)
    {
      var lines = input.GetLines();

      TreeNode rootNode = new TreeNode();
      int[] totalOnn = new int[lines[0].Length];
      int totalCount = lines.Count;

      foreach (string s in lines)
      {
        TreeNode currentNode;
        TreeNode nextNode = rootNode;

        for (int i = 0; i < s.Length; i++)
        {
          currentNode = nextNode;
          if (s[i] == '1')
          {
            totalOnn[i]++;
            nextNode = currentNode.GetOnnChild();
          }
          else
          {
            nextNode = currentNode.GetOffChild();
          }
          nextNode.count++;
        }
      }

      char[] mostCommon = new char[lines[0].Length];
      for (int i = 0; i < totalOnn.Length; i++)
      {
        mostCommon[i] = '0';
        if (totalOnn[i] > totalCount / 2)
        {
          mostCommon[i] = '1';
        }
      }

      int gammaValue = BinToInt(mostCommon, false);
      int epsilonValue = BinToInt(mostCommon, true);

      SubmitPartOne(gammaValue * epsilonValue);

      TreeNode oxygenCurrent = rootNode;
      while (oxygenCurrent.HasChild())
      {
        oxygenCurrent = oxygenCurrent.GetGreaterChild();
      }

      TreeNode carbonCurrent = rootNode;
      while (carbonCurrent.HasChild())
      {
        carbonCurrent = carbonCurrent.GetLesserChild();
      }

      int oxygenValue = oxygenCurrent.GetValue();
      int carbonValue = carbonCurrent.GetValue();

      SubmitPartTwo(oxygenValue * carbonValue);
    }

    private int BinToInt(char[] s, bool inverted = false)
    {
      int value = 0;
      for (int i = 0; i < s.Length; i++)
      {
        if ((!inverted && s[s.Length - 1 - i] == '1') || (inverted && s[s.Length - 1 - i] == '0'))
        {
          value += 1 << i;
        }
      }

      return value;
    }

    private class TreeNode
    {
      private TreeNode parent;
      private TreeNode onnChild;
      private TreeNode offChild;
      private bool isOnn;
      public int count;

      public TreeNode()
      {
        count = 0;
      }

      public TreeNode GetOnnChild()
      {
        if (onnChild == null)
        {
          onnChild = new TreeNode();
          onnChild.parent = this;
          onnChild.isOnn = true;
        }

        return onnChild;
      }

      public TreeNode GetOffChild()
      {
        if (offChild == null)
        {
          offChild = new TreeNode();
          offChild.parent = this;
          offChild.isOnn = false;
        }

        return offChild;
      }

      public bool HasChild()
      {
        return (onnChild != null || offChild != null);
      }

      public int GetValue()
      {
        int value = 0;
        if (parent != null)
          value += parent.GetValue() << 1;

        if (this.isOnn)
          value += 1;

        return value;
      }

      public TreeNode GetGreaterChild()
      {
        if (onnChild == null && offChild == null)
          return null;
        else if (onnChild == null && offChild != null)
          return offChild;
        else if (onnChild != null && offChild == null)
          return onnChild;
        else if (onnChild.count > offChild.count)
          return onnChild;
        else if (onnChild.count == offChild.count)
          return onnChild;
        else return offChild;
      }

      public TreeNode GetLesserChild()
      {
        if (onnChild == null && offChild == null)
          return null;
        else if (onnChild == null && offChild != null)
          return offChild;
        else if (onnChild != null && offChild == null)
          return onnChild;
        else if (onnChild.count < offChild.count)
          return onnChild;
        else return offChild;
      }
    }
  }
}