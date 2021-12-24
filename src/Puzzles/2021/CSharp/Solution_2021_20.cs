using AdventOfCode.Utils;
using System.Collections.Generic;
using System;
using System.Text;

namespace AdventOfCode.Solutions
{
  public class Solution_2021_20 : CSharpSolution
  {
    public override void Solve(PuzzleInput input)
    {
      var chunks = input.GetChunks();

      List<bool> lookupTable = new List<bool>();
      foreach (char c in chunks[0][0])
      {
        lookupTable.Add(c == '#');
      }

      Dictionary<Coordinate2D, bool> image = new Dictionary<Coordinate2D, bool>();

      int x = 0;
      int y = 0;

      foreach (string line in chunks[1])
      {
        x = 0;
        foreach (char c in line)
        {
          image[(x, y)] = c == '#';
          x++;
        }
        y++;
      }

      image = AddNeighbors(image);
      image = AddNeighbors(image);

      image = UpdateImage(image, lookupTable, false);
      image = UpdateImage(image, lookupTable, false);

      int onCount = 0;
      foreach (bool b in image.Values)
      {
        onCount += b ? 1 : 0;
      }

      SubmitPartOne(onCount);

      for (int i = 0; i < 24; i++)
      {
        image = UpdateImage(image, lookupTable, false);
        image = UpdateImage(image, lookupTable, false);
      }

      onCount = 0;
      foreach (bool b in image.Values)
      {
        onCount += b ? 1 : 0;
      }

      Console.Out.WriteLine(DisplayImage(image));

      SubmitPartTwo(onCount);
    }

    private string DisplayImage(Dictionary<Coordinate2D, bool> image)
    {
      List<Coordinate2D> mutableKeys = new List<Coordinate2D>();
      mutableKeys.AddRange(image.Keys);
      mutableKeys.Sort();

      StringBuilder sb = new StringBuilder();

      for (int y = mutableKeys[0].y; y <= mutableKeys[mutableKeys.Count - 1].y; y++)
      {
        for (int x = mutableKeys[0].x; x <= mutableKeys[mutableKeys.Count - 1].x; x++)
        {
          sb.Append(image.GetValueOrDefault((x, y), false) ? '#' : '.');
        }
        sb.Append('\n');
      }

      return sb.ToString();

    }

    private Dictionary<Coordinate2D, bool> AddNeighbors(Dictionary<Coordinate2D, bool> image)
    {
      Dictionary<Coordinate2D, bool> newImage = new Dictionary<Coordinate2D, bool>();
      HashSet<Coordinate2D> nearby = new HashSet<Coordinate2D>();

      foreach (Coordinate2D coord in image.Keys)
      {
        newImage[coord] = image[coord];

        foreach (Coordinate2D neighbor in Coordinate2D.neighborsDiagonal)
        {
          if (!image.ContainsKey(coord + neighbor))
          {
            newImage[coord + neighbor] = false;
          }
        }
      }

      return newImage;
    }

    private Dictionary<Coordinate2D, bool> UpdateImage(Dictionary<Coordinate2D, bool> oldImage, List<bool> lookupTable, bool unknowns)
    {
      Dictionary<Coordinate2D, bool> newImage = new Dictionary<Coordinate2D, bool>();
      HashSet<Coordinate2D> newTiles = new HashSet<Coordinate2D>();

      foreach (Coordinate2D coord in oldImage.Keys)
      {
        newImage[coord] = lookupTable[GetIndex(oldImage, coord, unknowns)];

        foreach (Coordinate2D neighbor in Coordinate2D.neighborsDiagonal)
        {
          if (!oldImage.ContainsKey(coord + neighbor))
          {
            newImage[coord + neighbor] = lookupTable[GetIndex(oldImage, coord + neighbor, unknowns)];

          }
        }
      }

      return newImage;
    }

    private int GetIndex(Dictionary<Coordinate2D, bool> image, Coordinate2D center, bool unknowns)
    {
      int index = 0;
      index += image.GetValueOrDefault(center + (-1, -1), unknowns) ? 1 : 0; index <<= 1;
      index += image.GetValueOrDefault(center + (0, -1), unknowns) ? 1 : 0; index <<= 1;
      index += image.GetValueOrDefault(center + (+1, -1), unknowns) ? 1 : 0; index <<= 1;
      index += image.GetValueOrDefault(center + (-1, 0), unknowns) ? 1 : 0; index <<= 1;
      index += image.GetValueOrDefault(center + (0, 0), unknowns) ? 1 : 0; index <<= 1;
      index += image.GetValueOrDefault(center + (+1, 0), unknowns) ? 1 : 0; index <<= 1;
      index += image.GetValueOrDefault(center + (-1, +1), unknowns) ? 1 : 0; index <<= 1;
      index += image.GetValueOrDefault(center + (0, +1), unknowns) ? 1 : 0; index <<= 1;
      index += image.GetValueOrDefault(center + (+1, +1), unknowns) ? 1 : 0;

      return index;
    }

    private class Coordinate2D : IComparable<Coordinate2D>
    {
      public int x;
      public int y;

      public static Coordinate2D[] neighborsLinear = { (1, 0), (0, 1), (-1, 0), (0, -1) };
      public static Coordinate2D[] neighborsDiagonal = { (1, 0), (1, 1), (0, 1), (-1, 1), (-1, 0), (-1, -1), (0, -1), (1, -1) };

      public Coordinate2D(int x, int y)
      {
        this.x = x;
        this.y = y;
      }

      public Coordinate2D Rotate(int rz)
      {
        (int x, int y) transformed = (x, y);

        for (int i = 0; i < rz % 4; i++)
        {
          int temp = transformed.x;
          transformed.x = -transformed.y;
          transformed.y = temp;
        }

        return transformed;
      }

      public override string ToString()
      {
        return "(" + x + "," + y + ")";
      }

      public int CompareTo(Coordinate2D other)
      {
        int comparison = this.y - other.y;
        if (comparison != 0) return Math.Sign(comparison);
        else return Math.Sign(this.x - other.x);
      }

      public override bool Equals(Object other)
      {
        if (!other.GetType().Equals(this.GetType()))
          return false;

        Coordinate2D o = (Coordinate2D)other;
        return (x == o.x && y == o.y);
      }

      public override int GetHashCode()
      {
        return (x, y).GetHashCode();
      }

      public static Coordinate2D operator +(Coordinate2D a) => a;
      public static Coordinate2D operator -(Coordinate2D a) => new Coordinate2D(-a.x, -a.y);
      public static Coordinate2D operator +(Coordinate2D a, Coordinate2D b) => new Coordinate2D(a.x + b.x, a.y + b.y);
      public static Coordinate2D operator -(Coordinate2D a, Coordinate2D b) => a + (-b);

      public static implicit operator (int x, int y)(Coordinate2D c) => (c.x, c.y);
      public static implicit operator Coordinate2D((int x, int y) tuple) => new Coordinate2D(tuple.x, tuple.y);
    }
  }
}