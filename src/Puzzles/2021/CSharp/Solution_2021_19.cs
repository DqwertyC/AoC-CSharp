using AdventOfCode.Utils;
using System.Collections.Generic;
using System;

namespace AdventOfCode.Solutions
{
  public class Solution_2021_19 : CSharpSolution
  {
    public override void Solve(PuzzleInput input)
    {
      var chunks = input.GetChunks();

      List<ScanResults> scanners = new List<ScanResults>();
      HashSet<ScanResults> unvisited = new HashSet<ScanResults>();

      foreach (List<string> chunk in chunks)
      {
        ScanResults scan = new ScanResults(chunk);
        scanners.Add(scan);
        unvisited.Add(scan);
      }

      List<ScanResults> overlapList = new List<ScanResults>();
      overlapList.Add(scanners[0]);
      unvisited.Remove(scanners[0]);

      int i = 0;

      while (overlapList.Count < scanners.Count)
      {
        ScanResults current = overlapList[i];

        foreach (ScanResults next in unvisited)
        {
          if (TryFindOverlap(current, next))
          {
            overlapList.Add(next);
            unvisited.Remove(next);
          }
        }
        i++;
      }

      HashSet<Coordinate> masterList = new HashSet<Coordinate>();

      foreach (ScanResults s in overlapList)
      {
        foreach (Coordinate c in s.beaconCoords)
        {
          masterList.Add(c);
        }
      }

      SubmitPartOne(masterList.Count);

      int maxDelta = 0;

      foreach (ScanResults a in overlapList)
      {
        foreach (ScanResults b in overlapList)
        {
          int delta = Math.Abs(a.offset.x - b.offset.x) + Math.Abs(a.offset.y - b.offset.y) + Math.Abs(a.offset.z - b.offset.z);
          maxDelta = delta > maxDelta ? delta : maxDelta;
        }
      }

      SubmitPartTwo(maxDelta);
    }

    private bool TryFindOverlap(ScanResults fixedScan, ScanResults unfixedScan)
    {
      List<Coordinate> coordsA = fixedScan.beaconCoords;
      List<Coordinate> coordsB = unfixedScan.beaconCoords;
      List<Coordinate> rotations = Coordinate.GetUniqueRotations();

      foreach (Coordinate rotation in rotations)
      {
        List<Coordinate> rotatedCoords = new List<Coordinate>();

        foreach (Coordinate coord in coordsB)
        {
          rotatedCoords.Add(coord.Rotate(rotation.x, rotation.y, rotation.z));
        }

        foreach (Coordinate a in coordsA)
        {
          foreach (Coordinate b in rotatedCoords)
          {
            Coordinate delta = a - b;
            int matchCount = 0;

            foreach (Coordinate c in rotatedCoords)
            {
              matchCount += coordsA.Contains(c + delta) ? 1 : 0;
            }

            if (matchCount >= 12)
            {
              for (int i = 0; i < rotatedCoords.Count; i++)
              {
                rotatedCoords[i] += delta;
              }

              unfixedScan.beaconCoords = rotatedCoords;
              unfixedScan.offset = delta;
              unfixedScan.rotation = rotation;
              return true;
            }
          }
        }
      }

      return false;
    }

    private class ScanResults
    {
      public List<Coordinate> beaconCoords;
      private List<Coordinate> rawCoords;
      public Coordinate offset;
      public Coordinate rotation;

      public ScanResults(List<string> s)
      {
        beaconCoords = new List<Coordinate>();
        rawCoords = new List<Coordinate>();

        offset = (0, 0, 0);
        rotation = (0, 0, 0);

        for (int i = 1; i < s.Count; i++)
        {
          string[] coordParts = s[i].Split(",");
          Coordinate coord = new Coordinate(int.Parse(coordParts[0]), int.Parse(coordParts[1]), int.Parse(coordParts[2]));
          beaconCoords.Add(coord);
          rawCoords.Add(coord);
        }
      }
    }

    private class Coordinate
    {
      public int x;
      public int y;
      public int z;

      public Coordinate(int x, int y, int z = 0)
      {
        this.x = x;
        this.y = y;
        this.z = z;
      }

      public Coordinate Rotate(int rx, int ry, int rz)
      {
        (int x, int y, int z) transformed = (x, y, z);

        for (int i = 0; i < rx % 4; i++)
        {
          int temp = transformed.y;
          transformed.y = -transformed.z;
          transformed.z = temp;
        }

        for (int i = 0; i < ry % 4; i++)
        {
          int temp = transformed.z;
          transformed.z = -transformed.x;
          transformed.x = temp;
        }

        for (int i = 0; i < rz % 4; i++)
        {
          int temp = transformed.x;
          transformed.x = -transformed.y;
          transformed.y = temp;
        }

        return transformed;
      }

      public HashSet<Coordinate> Transforms()
      {
        HashSet<Coordinate> possibleTransforms = new HashSet<Coordinate>();

        for (int rx = 0; rx < 4; rx++)
        {
          for (int ry = 0; ry < 4; ry++)
          {
            for (int rz = 0; rz < 4; rz++)
            {
              possibleTransforms.Add(Rotate(rx, ry, rz));
            }
          }
        }

        return possibleTransforms;
      }

      private static bool rotationsCalculated = false;
      private static List<Coordinate> uniqueRotations;
      public static List<Coordinate> GetUniqueRotations()
      {
        if (uniqueRotations != null) return uniqueRotations;

        uniqueRotations = new List<Coordinate>();

        HashSet<Coordinate> possibleTransforms = new HashSet<Coordinate>();
        Coordinate baseCoordinates = new Coordinate(1, 2, 3);
        for (int rx = 0; rx < 4; rx++)
        {
          for (int ry = 0; ry < 4; ry++)
          {
            for (int rz = 0; rz < 4; rz++)
            {
              Coordinate rotatedCoordinates = baseCoordinates.Rotate(rx, ry, rz);
              if (!possibleTransforms.Contains(rotatedCoordinates))
              {
                possibleTransforms.Add(rotatedCoordinates);
                uniqueRotations.Add(new Coordinate(rx, ry, rz));
              }
            }
          }
        }

        return uniqueRotations;
      }

      public override string ToString()
      {
        return "(" + x + "," + y + "," + z + ")";
      }

      public override bool Equals(Object other)
      {
        if (!other.GetType().Equals(this.GetType()))
          return false;

        Coordinate o = (Coordinate)other;
        return (x == o.x && y == o.y && z == o.z);
      }

      public override int GetHashCode()
      {
        return (x, y, z).GetHashCode();
      }

      public static Coordinate operator +(Coordinate a) => a;
      public static Coordinate operator -(Coordinate a) => new Coordinate(-a.x, -a.y, -a.z);
      public static Coordinate operator +(Coordinate a, Coordinate b) => new Coordinate(a.x + b.x, a.y + b.y, a.z + b.z);
      public static Coordinate operator -(Coordinate a, Coordinate b) => a + (-b);

      public static implicit operator (int x, int y, int z)(Coordinate c) => (c.x, c.y, c.z);
      public static implicit operator Coordinate((int x, int y, int z) thruple) => new Coordinate(thruple.x, thruple.y, thruple.z);
    }
  }
}