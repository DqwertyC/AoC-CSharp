using AdventOfCode.Utils;
using System.Collections.Generic;

namespace AdventOfCode.Solutions
{
  public class Solution_2021_04 : CSharpSolution
  {
    public override void Solve(PuzzleInput input)
    {
      // Write your puzzle solution here!
      var chunks = input.GetChunks();

      List<int> bingoInputs = new PuzzleInput(chunks[0][0]).GetInts(",");
      HashSet<BingoBoard> remainingBoards = new HashSet<BingoBoard>();

      for (int i = 1; i < chunks.Count; i++)
      {
        remainingBoards.Add(new BingoBoard(chunks[i]));
      }

      bool firstBingo = false;
      int index = 0;

      HashSet<BingoBoard> toRemove = new HashSet<BingoBoard>();

      while (remainingBoards.Count > 0)
      {
        foreach (BingoBoard board in remainingBoards)
        {
          if (board.MarkAndCheck(bingoInputs[index]))
          {
            if (!firstBingo)
            {
              firstBingo = true;
              SubmitPartOne(board.GetScore(bingoInputs[index]));
            }

            if (remainingBoards.Count == 1)
            {
              SubmitPartTwo(board.GetScore(bingoInputs[index]));
            }

            toRemove.Add(board);
          }
        }

        foreach (BingoBoard board in toRemove)
        {
          remainingBoards.Remove(board);
        }

        index++;
      }
    }

    private class BingoBoard
    {
      private const int gridSize = 5;
      HashSet<(int, int)> markedTiles;
      Dictionary<int, (int, int)> tileLocations;

      public BingoBoard(List<string> input)
      {
        markedTiles = new HashSet<(int, int)>();
        tileLocations = new Dictionary<int, (int, int)>();

        int y = 0;
        foreach (string line in input)
        {
          int x = 0;
          foreach (string val in line.Split(" ", System.StringSplitOptions.RemoveEmptyEntries))
          {
            tileLocations[int.Parse(val)] = (x, y);
            x++;
          }
          y++;
        }
      }

      public bool MarkAndCheck(int val)
      {
        bool bingo = false;
        if (tileLocations.ContainsKey(val))
        {
          (int x, int y) location = tileLocations[val];
          markedTiles.Add(location);

          bool rowFilled = true;
          bool colFilled = true;

          // Check rows and cols
          for (int i = 0; i < gridSize; i++)
          {
            rowFilled = rowFilled && markedTiles.Contains((i, location.y));
            colFilled = colFilled && markedTiles.Contains((location.x, i));
          }

          bingo = rowFilled || colFilled;
        }
        return bingo;
      }

      public int GetScore(int lastNumber)
      {
        int sum = 0;
        foreach (int val in tileLocations.Keys)
        {
          if (!markedTiles.Contains(tileLocations[val]))
          {
            sum += val;
          }
        }

        return sum * lastNumber;
      }
    }
  }
}