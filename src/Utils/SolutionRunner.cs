using AdventOfCode.Solutions;
using System.Diagnostics;
using System;

namespace AdventOfCode.Utils
{
  public static class SolutionRunner
  {
    private static Stopwatch _timer;
    private static string _part1Answer;
    private static string _part2Answer;
    private static long _part1Time;
    private static long _part2Time;
    private static bool _part1Solved;
    private static bool _part2Solved;
    public delegate void cDelSubmit(object solution);
    public static cDelSubmit cDelOne = CSubmitPartOne;
    public static cDelSubmit cDelTwo = CSubmitPartTwo;

    public static void RunSolver(int year, int day)
    {
      _timer = new Stopwatch();
      _part1Answer = string.Empty;
      _part2Answer = string.Empty;
      _part1Solved = false;
      _part2Solved = false;
      _part1Time = 0;
      _part2Time = 0;

      PuzzleInput input = new PuzzleInput(year, day);
      SolveCSharp(input, year, day);
    }

    public static void SolveCSharp(PuzzleInput input, int year, int day)
    {
      // Get the name of the class for the given year and day
      string typeName = $"AdventOfCode.Solutions.Solution_{year}_{day:D2}";
      Type typeType = Type.GetType(typeName);
      CSharpSolution solution = (CSharpSolution)Activator.CreateInstance(typeType);

      _timer.Start();
      solution.Solve(input);
      _timer.Stop();
    }

    public static (string solution, double time) GetPartOne()
    {
      if (_part1Solved)
      {
        return (_part1Answer, _part1Time);
      }
      else
      {
        return ("Unsolved", 0);
      }
    }

    public static (string solution, double time) GetPartTwo()
    {
      if (_part2Solved)
      {
        return (_part2Answer, _part2Time);
      }
      else
      {
        return ("Unsolved", 0);
      }
    }

    private static void CSubmitPartOne(object answer)
    {
      _part1Time = _timer.ElapsedTicks;
      _part1Answer = answer.ToString();
      _part1Solved = true;
    }

    private static void CSubmitPartTwo(object answer)
    {
      _part2Time = _timer.ElapsedTicks;
      _part2Answer = answer.ToString();
      _part2Solved = true;
    }
  }
}
