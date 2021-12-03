using AdventOfCode.Utils;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Solutions
{
  public class Solution_2021_03a : CSharpSolution
  {
    string demo_input = "00100\n11110\n10110\n10111\n10101\n01111\n00111\n11100\n10000\n11001\n00010\n01010";

    public override void Solve(PuzzleInput input)
    {
      var lines = input.GetLines();
      //List<string> lines = new List<string>();
      //lines.AddRange(demo_input.Split("\n"));

      List<string> oxygenCandidates = new List<string>();
      List<string> carbonCandidates = new List<string>();

      int minValue = 0;
      int maxValue = 0;

      char[] minString = new char[lines[0].Length];
      char[] maxString = new char[lines[0].Length];

      for (int i = 0; i < lines[0].Length; i++)
      {
        (int onnCount, int offCount) = GetOnOffCount(lines, i);

        if (onnCount > offCount)
        {
          minString[i] = '0';
          maxString[i] = '1';
        }
        else
        {
          minString[i] = '1';
          maxString[i] = '0';
        }
      }

      minValue = ValueFromBinary(minString);
      maxValue = ValueFromBinary(maxString);

      SubmitPartOne(minValue * maxValue);

      oxygenCandidates.AddRange(lines);
      carbonCandidates.AddRange(lines);

      int scanIndex = 0;
      while (oxygenCandidates.Count > 1)
      {
        (int onnCount, int offCount) = GetOnOffCount(oxygenCandidates, scanIndex);
        char expected = (onnCount >= offCount) ? '1' : '0';
        List<string> toRemove = new List<string>();

        foreach (string s in oxygenCandidates)
        {
          if (s[scanIndex] != expected)
          {
            toRemove.Add(s);
          }
        }

        foreach (string s in toRemove)
        {
          oxygenCandidates.Remove(s);
        }

        scanIndex++;
      }

      scanIndex = 0;
      while (carbonCandidates.Count > 1)
      {
        (int onnCount, int offCount) = GetOnOffCount(carbonCandidates, scanIndex);
        char expected = (onnCount < offCount) ? '1' : '0';
        List<string> toRemove = new List<string>();

        foreach (string s in carbonCandidates)
        {
          if (s[scanIndex] != expected)
          {
            toRemove.Add(s);
          }
        }

        foreach (string s in toRemove)
        {
          carbonCandidates.Remove(s);
        }

        scanIndex++;
      }

      int oxygenValue = ValueFromBinary(oxygenCandidates[0].ToCharArray());
      int carbonValue = ValueFromBinary(carbonCandidates[0].ToCharArray());

      SubmitPartTwo(oxygenValue * carbonValue);
    }

    private int ValueFromBinary(char[] s)
    {
      int value = 0;
      for (int i = 0; i < s.Length; i++)
      {
        if (s[s.Length - 1 - i] == '1')
        {
          value += 1 << i;
        }
      }

      return value;
    }

    private (int on, int off) GetOnOffCount(List<string> lines, int index)
    {
      int onnCount = 0;
      int offCount = 0;

      foreach (string s in lines)
      {
        if (s[index] == '1')
        {
          onnCount++;
        }
        else
        {
          offCount++;
        }
      }

      return (onnCount, offCount);
    }
  }
}