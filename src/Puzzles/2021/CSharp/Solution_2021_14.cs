using AdventOfCode.Utils;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Solutions
{
  public class Solution_2021_14 : CSharpSolution
  {
    public override void Solve(PuzzleInput input)
    {
      var chunks = input.GetChunks();
      string polymer = chunks[0][0];
      int maxIndex = new CharPair('Z', 'Z').GetIndex();

      //Dictionary<CharPair, char> expansionRules = new Dictionary<CharPair, char>();
      //Dictionary<CharPair, long> pairCounts = new Dictionary<CharPair, long>();
      //Dictionary<char, long> charCounts = new Dictionary<char, long>();

      HashSet<CharPair> possiblePairs = new HashSet<CharPair>();
      char[] expansionRules = new char[maxIndex + 1];
      long[] pairCounts = new long[maxIndex + 1];
      long[] dupedCharCounts = new long['Z' + 1];

      foreach (string line in chunks[1])
      {
        CharPair rulePair = new CharPair(line[0], line[1]);
        possiblePairs.Add(rulePair);

        expansionRules[rulePair.GetIndex()] = line[6];
        pairCounts[rulePair.GetIndex()] = 0;
        dupedCharCounts[line[6]] = 0;
      }

      for (int i = 0; i < polymer.Length - 1; i++)
      {
        pairCounts[new CharPair(polymer[i], polymer[i + 1]).GetIndex()]++;

        if (i > 0)
        {
          dupedCharCounts[polymer[i]]++;
        }
      }

      for (int i = 0; i < 40; i++)
      {
        if (i == 10)
        {
          SubmitPartOne(GetScore(dupedCharCounts, pairCounts, possiblePairs));
        }

        long[] newPairCounts = new long[pairCounts.Length];

        foreach (CharPair pair in possiblePairs)
        {
          long count = pairCounts[pair.GetIndex()];
          char c = expansionRules[pair.GetIndex()];
          CharPair newPairA = new CharPair(pair.a, c);
          CharPair newPairB = new CharPair(c, pair.b);

          newPairCounts[newPairA.GetIndex()] += count;
          newPairCounts[newPairB.GetIndex()] += count;
          dupedCharCounts[c] += count;
        }

        pairCounts = newPairCounts;
      }

      SubmitPartTwo(GetScore(dupedCharCounts, pairCounts, possiblePairs));

    }

    private long GetScore(long[] dupedCharCounts, long[] pairCounts, HashSet<CharPair> possiblePairs)
    {

      long[] charCounts = new long['Z' + 1];

      foreach (CharPair pair in possiblePairs)
      {
        charCounts[pair.a] += pairCounts[pair.GetIndex()];
        charCounts[pair.b] += pairCounts[pair.GetIndex()];
      }

      char maxChar = '\0';
      long maxCount = 0;

      char minChar = '\0';
      long minCount = long.MaxValue;

      for (char c = 'A'; c <= 'Z'; c++)
      {
        charCounts[c] -= dupedCharCounts[c];

        if (charCounts[c] < minCount && charCounts[c] > 0)
        {
          minCount = charCounts[c];
          minChar = c;
        }

        if (charCounts[c] > maxCount)
        {
          maxCount = charCounts[c];
          maxChar = c;
        }
      }

      return maxCount - minCount;
    }

    private class CharPair
    {
      public char a;
      public char b;

      public CharPair(char c, char d)
      {
        a = c;
        b = d;
      }

      public override int GetHashCode()
      {
        return this.GetIndex();
      }

      public int GetIndex()
      {
        return ('Z' - 'A' + 1) * (a - 'A') + (b - 'A');
      }

      public override bool Equals(object obj)
      {
        if (obj.GetType() == this.GetType())
        {
          return (this.a == ((CharPair)obj).a) && (this.b == ((CharPair)obj).b);
        }
        return false;
      }
    }
  }
}
