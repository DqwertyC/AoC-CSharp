using AdventOfCode.Utils;
using System.Collections.Generic;

namespace AdventOfCode.Solutions
{
  public class Solution_2021_12 : CSharpSolution
  {

    string testA = @"start-A
start-b
A-c
A-b
b-d
A-end
b-end";

    string testB = @"dc-end
HN-start
start-kj
dc-start
dc-HN
LN-dc
HN-end
kj-sa
kj-HN
kj-dc";

    string testC = @"fs-end
he-DX
fs-he
start-DX
pj-DX
end-zg
zg-sl
zg-pj
pj-he
RW-he
fs-DX
pj-RW
zg-RW
start-pj
he-WI
zg-he
pj-fs
start-RW";

    public override void Solve(PuzzleInput input)
    {
      PuzzleInput testInputA = new PuzzleInput(testA);
      PuzzleInput testInputB = new PuzzleInput(testB);
      PuzzleInput testInputC = new PuzzleInput(testC);

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

      int longPaths = 0;
      int shortPaths = 0;
      Queue<(List<CaveGraphNode>, bool)> currentPaths = new Queue<(List<CaveGraphNode>, bool)>();
      List<CaveGraphNode> startPath = new List<CaveGraphNode>();
      startPath.Add(nodes["start"]);
      currentPaths.Enqueue((startPath, false));

      while (currentPaths.Count > 0)
      {
        (List<CaveGraphNode> currentPath, bool usedRevisit) = currentPaths.Dequeue();

        foreach (CaveGraphNode next in currentPath[currentPath.Count - 1].neighbors)
        {
          bool requiresRevisit = next.isSmallCave && currentPath.Contains(next);

          if (!requiresRevisit || !usedRevisit)
          {
            List<CaveGraphNode> nextPath = new List<CaveGraphNode>(currentPath);
            nextPath.Add(next);

            if (next.isEnd)
            {
              longPaths++;
              shortPaths += usedRevisit ? 0 : 1;
            }
            else
            {
              currentPaths.Enqueue((nextPath, usedRevisit || requiresRevisit));
            }
          }
        }
      }

      SubmitPartOne(shortPaths);
      SubmitPartTwo(longPaths);
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