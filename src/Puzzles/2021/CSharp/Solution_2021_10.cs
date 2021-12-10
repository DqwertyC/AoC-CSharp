using AdventOfCode.Utils;
using System.Collections.Generic;

namespace AdventOfCode.Solutions
{
  public class Solution_2021_10 : CSharpSolution
  {

    private Dictionary<char, char> chunkCloser;
    private Dictionary<char, int> corruptScores;
    private Dictionary<char, int> incompleteScores;

    public override void Solve(PuzzleInput input)
    {
      chunkCloser = new Dictionary<char, char>();
      chunkCloser['('] = ')';
      chunkCloser['['] = ']';
      chunkCloser['{'] = '}';
      chunkCloser['<'] = '>';

      corruptScores = new Dictionary<char, int>();
      corruptScores[')'] = 3;
      corruptScores[']'] = 57;
      corruptScores['}'] = 1197;
      corruptScores['>'] = 25137;

      incompleteScores = new Dictionary<char, int>();
      incompleteScores[')'] = 1;
      incompleteScores[']'] = 2;
      incompleteScores['}'] = 3;
      incompleteScores['>'] = 4;

      long corruptSum = 0;
      List<long> validScores = new List<long>();
      foreach (string line in input.GetLines())
      {
        if (GetScore(line, out long score))
        {
          corruptSum += score;
        }
        else
        {
          validScores.Add(score);
        }
      }

      SubmitPartOne(corruptSum);
      validScores.Sort();
      SubmitPartTwo(validScores[validScores.Count / 2]);
    }

    private bool GetScore(string line, out long score)
    {
      Stack<char> closerStack = new Stack<char>();

      for (int i = 0; i < line.Length; i++)
      {
        char c = line[i];

        if (chunkCloser.ContainsKey(c))
        {
          closerStack.Push(chunkCloser[c]);
        }
        else
        {
          if (closerStack.Peek() == c)
          {
            closerStack.Pop();
          }
          else
          {
            score = corruptScores[c];
            return true;
          }
        }
      }

      score = 0;
      while (closerStack.Count > 0)
      {
        score *= 5;
        score += incompleteScores[closerStack.Pop()];
      }

      return false;

    }
  }
}