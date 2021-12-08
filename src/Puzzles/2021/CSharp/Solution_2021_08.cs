using AdventOfCode.Utils;
using System.Collections.Generic;
using System;

namespace AdventOfCode.Solutions
{
  public class Solution_2021_08 : CSharpSolution
  {
    private static Dictionary<string, int> defaultDigits;

    public override void Solve(PuzzleInput input)
    {
      defaultDigits = new Dictionary<string, int>();

      string defaultWireString = "abcefg cf acdeg acdfg bcdf abdfg abdefg acf abcdefg abcdfg";
      string[] defaultDigitStrings = defaultWireString.Split(" ");

      for (int i = 0; i < defaultDigitStrings.Length; i++)
      {
        defaultDigits[defaultDigitStrings[i]] = i;
      }

      WireHelper defaultWires = new WireHelper(defaultWireString);

      int uniqueCount = 0;
      long totalSum = 0;

      foreach (string line in input.GetLines())
      {
        string[] lineParts = line.Split(" | ");
        WireHelper wires = new WireHelper(lineParts[0]);
        (int count, int value) = ParseValue(lineParts[1], wires.GenerateMap(defaultWires));

        uniqueCount += count;
        totalSum += value;
      }

      SubmitPartOne(uniqueCount);
      SubmitPartTwo(totalSum);
    }

    public (int, int) ParseValue(string values, Dictionary<char, char> conversion)
    {
      int uniqueCount = 0;
      int value = 0;
      foreach (string entry in values.Split(" "))
      {
        List<char> converted = new List<char>();
        foreach (char c in entry)
        {
          converted.Add(conversion[c]);
        }
        converted.Sort();

        int digit = defaultDigits[new string(converted.ToArray())];

        if (digit == 1 || digit == 4 || digit == 7 || digit == 8)
        {
          uniqueCount++;
        }

        value = 10 * value + digit;
      }

      return (uniqueCount, value);
    }

    private class WireHelper
    {
      public Dictionary<(char, int), int> wireCounts;

      public WireHelper(string input)
      {
        wireCounts = new Dictionary<(char, int), int>();

        for (char c = 'a'; c <= 'g'; c++)
        {
          wireCounts[(c, 3)] = 0;
          wireCounts[(c, 5)] = 0;
          wireCounts[(c, 6)] = 0;
        }

        foreach (string wireGroup in input.Split(" ", StringSplitOptions.RemoveEmptyEntries))
        {
          int length = wireGroup.Length;
          if (length == 3 || length == 5 || length == 6)
          {
            foreach (char c in wireGroup)
            {
              wireCounts[(c, length)] = wireCounts[(c, length)] + 1;
            }
          }
        }
      }

      public Dictionary<char, char> GenerateMap(WireHelper other)
      {
        Dictionary<char, char> conversionTable = new Dictionary<char, char>();

        for (char thisC = 'a'; thisC <= 'g'; thisC++)
        {
          for (char otherC = 'a'; otherC <= 'g'; otherC++)
          {
            if (this.wireCounts[(thisC, 3)] == other.wireCounts[(otherC, 3)] &&
                this.wireCounts[(thisC, 5)] == other.wireCounts[(otherC, 5)] &&
                this.wireCounts[(thisC, 6)] == other.wireCounts[(otherC, 6)])
            {
              conversionTable[thisC] = otherC;
            }
          }
        }

        return conversionTable;
      }
    }
  }
}