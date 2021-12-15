using AdventOfCode.Utils;
using System.Collections.Generic;

namespace AdventOfCode.Solutions
{
  public class Solution_2021_12 : CSharpSolution
  {


    public override void Solve(PuzzleInput input)
    {
      Dictionary<string, CaveGraphNode> nodes = new Dictionary<string, CaveGraphNode>();

      foreach (string line in input.GetLines())
      {
        string[] lineNodes = line.Split("-");

        if (!nodes.ContainsKey(lineNodes[0]))
        {
          nodes[lineNodes[0]] = new CaveGraphNode(lineNodes[0]);
        }

        if (!nodes.ContainsKey(lineNodes[1]))
        {
          nodes[lineNodes[1]] = new CaveGraphNode(lineNodes[1]);
        }

        if (!lineNodes[0].Equals("end") && !lineNodes[1].Equals("start"))
        {
          nodes[lineNodes[0]].neighbors.Add(nodes[lineNodes[1]]);
        }

        if (!lineNodes[1].Equals("end") && !lineNodes[0].Equals("start"))
        {
          nodes[lineNodes[1]].neighbors.Add(nodes[lineNodes[0]]);
        }
      }

      currentSmallNodes = new HashSet<CaveGraphNode>();
      currentPath = new Stack<CaveGraphNode>();
      currentPath.Push(nodes["start"]);
      currentDuplicate = null;

      shortPaths = 0;
      longPaths = 0;

      RecurseCaves();

      SubmitPartOne(shortPaths);
      SubmitPartTwo(longPaths);
    }

    private Stack<CaveGraphNode> currentPath;
    private HashSet<CaveGraphNode> currentSmallNodes;
    private CaveGraphNode currentDuplicate = null;

    private int shortPaths;
    private int longPaths;

    private void RecurseCaves()
    {
      foreach (CaveGraphNode next in currentPath.Peek().neighbors)
      {
        bool willRevisit = currentSmallNodes.Contains(next);
        if (!next.isSmallCave || !currentSmallNodes.Contains(next) || currentDuplicate == null)
        {
          if (currentSmallNodes.Contains(next))
          {
            currentDuplicate = next;
          }

          if (next.isSmallCave)
          {
            currentSmallNodes.Add(next);
          }

          currentPath.Push(next);
          RecurseCaves();
        }
      }

      CaveGraphNode top = currentPath.Pop();

      if (top.isEnd)
      {
        longPaths++;
        shortPaths += currentDuplicate == null ? 1 : 0;
      }

      if (top.isSmallCave)
      {
        if (currentDuplicate == top)
        {
          currentDuplicate = null;
        }
        else
        {
          currentSmallNodes.Remove(top);
        }
      }
    }

    private bool CheckUsedRevisit(List<CaveGraphNode> path)
    {
      foreach (CaveGraphNode node in path)
      {
        if (node.isSmallCave && path.LastIndexOf(node) != path.IndexOf(node))
        {
          return true;
        }
      }
      return false;
    }

    private class CaveGraphNode
    {
      public HashSet<CaveGraphNode> neighbors;
      public bool isSmallCave;
      public bool isStart;
      public bool isEnd;
      public string name;

      public CaveGraphNode(string s)
      {
        name = s;
        isSmallCave = s[0] >= 'a' && s[0] <= 'z';
        isStart = s.Equals("start");
        isEnd = s.Equals("end");
        neighbors = new HashSet<CaveGraphNode>();
      }
    }
  }
}