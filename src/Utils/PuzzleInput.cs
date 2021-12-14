using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AdventOfCode.Utils
{
  public class PuzzleInput
  {
    public string rawInput = string.Empty;

    public PuzzleInput(int year, int day)
    {
      if (File.Exists(IOUtils.InputPath(year, day)) || InputGrabber.TryFetchInput(year, day))
      {
        rawInput = File.ReadAllText(IOUtils.InputPath(year, day));
      }
    }

    public PuzzleInput(string raw)
    {
      rawInput = raw;
    }

    public string GetRaw()
    {
      return rawInput;
    }

    public override string ToString()
    {
      return rawInput;
    }

    public List<char> GetChars()
    {
      return new List<char>(rawInput.ToCharArray());
    }

    public Dictionary<(int, int), char> GetDictionary()
    {
      Dictionary<(int, int), char> dictionary = new Dictionary<(int, int), char>();
      int x;
      int y = 0;

      foreach (string line in GetLines())
      {
        x = 0;
        foreach (char c in line)
        {
          dictionary[(x, y)] = c;
          x++;
        }
        y++;
      }

      return dictionary;
    }

    public Dictionary<(int, int), int> GetDictionaryInt()
    {
      Dictionary<(int, int), int> dictionary = new Dictionary<(int, int), int>();
      int x;
      int y = 0;

      foreach (string line in GetLines())
      {
        x = 0;
        foreach (char c in line)
        {
          dictionary[(x, y)] = c - '0';
          x++;
        }
        y++;
      }

      return dictionary;
    }

    public List<int> GetInts(string separator)
    {
      List<int> ints = new List<int>();
      foreach (string s in rawInput.Split(separator, StringSplitOptions.RemoveEmptyEntries))
      {
        int temp;
        if (int.TryParse(s, out temp))
        {
          ints.Add(temp);
        }
      }

      return ints;
    }

    public List<long> GetLongs(string separator)
    {
      List<long> longs = new List<long>();
      foreach (string s in rawInput.Split(separator, StringSplitOptions.RemoveEmptyEntries))
      {
        long temp;
        if (long.TryParse(s, out temp))
        {
          longs.Add(temp);
        }
      }

      return longs;
    }

    public List<string> GetLines()
    {
      var lines = new List<string>(rawInput.Split("\n", StringSplitOptions.RemoveEmptyEntries));
      return lines;
    }

    public List<List<string>> GetChunks()
    {
      List<List<string>> chunks = new List<List<string>>();

      foreach (string chunk in rawInput.Split("\n\n", StringSplitOptions.RemoveEmptyEntries))
      {
        List<string> chunkLines = new List<string>(chunk.Split("\n", StringSplitOptions.RemoveEmptyEntries));
        chunks.Add(chunkLines);
      }

      return chunks;
    }
  }
}
