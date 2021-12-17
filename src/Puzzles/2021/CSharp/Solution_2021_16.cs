using AdventOfCode.Utils;
using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Solutions
{
  public class Solution_2021_16 : CSharpSolution
  {

    string testString = "D2FE28";
    public override void Solve(PuzzleInput input)
    {

      List<byte> binary = new List<byte>();

      foreach (char c in testString)//input.GetRaw())
      {
        long val = c >= '0' && c <= '9' ? c - 0 : c - 'A' + 10;

        binary.Add((byte)((val & 8) != 0 ? 1 : 0));
        binary.Add((byte)((val & 4) != 0 ? 1 : 0));
        binary.Add((byte)((val & 2) != 0 ? 1 : 0));
        binary.Add((byte)((val & 1) != 0 ? 1 : 0));
      }

      Packet root = new Packet(binary);

      SubmitPartOne(root.GetPacketSum());
      SubmitPartTwo(root.GetPacketValue());
    }

    private class Packet
    {
      long version;
      long packetType;
      long sizeType;
      long literalVal;
      int length;
      List<Packet> children;

      public Packet(List<byte> raw)
      {
        version = (4 * raw[0] + 2 * raw[1] + raw[2]);
        packetType = (4 * raw[3] + 2 * raw[4] + raw[5]);
        length = 6;

        if (packetType == 4)
        {
          length = 1;
          literalVal = 0;

          do
          {
            length += 5;
            literalVal *= 16;
            literalVal += 8 * raw[length + 1] + 4 * raw[length + 2] + 2 * raw[length + 3] + 1 * raw[length + 4];
          }
          while (raw[length] == 1);
          length += 5;
        }
        else
        {
          length += 1;
          children = new List<Packet>();

          long maxChildren = long.MaxValue;
          long maxBits = long.MaxValue;

          if (raw[6] == 0)
          {
            //15 bit longeger describing length of sub packets
            length += 15;
            maxBits = 0;

            for (int i = 0; i < 15; i++)
            {
              maxBits *= 2;
              maxBits += raw[7 + i];
            }
          }
          else
          {
            //11 bit longeger describing number of sub packets
            length += 11;
            maxChildren = 0;

            for (int i = 0; i < 11; i++)
            {
              maxChildren *= 2;
              maxChildren += raw[7 + i];
            }
          }

          long bitCount = 0;
          long childCount = 0;

          while (childCount < maxChildren && bitCount < maxBits)
          {
            List<byte> newRaw = new List<byte>(raw);
            newRaw.RemoveRange(0, length);
            Packet newChild = new Packet(newRaw);

            bitCount += newChild.length;
            length += newChild.length;
            childCount += 1;
            children.Add(newChild);
          }
        }

        raw.RemoveRange(length, raw.Count - length);
      }

      public long GetPacketSum()
      {
        long sum = version;
        if (children != null)
        {
          foreach (Packet child in children)
          {
            sum += child.GetPacketSum();
          }
        }

        return sum;
      }

      public long GetPacketValue()
      {
        if (packetType == 4)
        {
          return literalVal;
        }

        List<long> childrenVals = new List<long>();
        foreach (Packet child in children)
        {
          childrenVals.Add(child.GetPacketValue());
        }

        long value = 0;

        switch (packetType)
        {
          case 0:
            value = 0;
            foreach (long i in childrenVals)
            {
              value += i;
            }
            break;
          case 1:
            value = 1;
            foreach (long i in childrenVals)
            {
              value = value * i;
            }
            break;
          case 2:
            childrenVals.Sort();
            value = childrenVals[0];
            break;
          case 3:
            childrenVals.Sort();
            childrenVals.Reverse();
            value = childrenVals[0];
            break;
          case 5:
            value = childrenVals[0] > childrenVals[1] ? 1 : 0;
            break;
          case 6:
            value = childrenVals[0] < childrenVals[1] ? 1 : 0;
            break;
          case 7:
            value = childrenVals[0] == childrenVals[1] ? 1 : 0;
            break;
          default:
            value = -1;
            break;
        }

        return value;
      }
    }
  }
}