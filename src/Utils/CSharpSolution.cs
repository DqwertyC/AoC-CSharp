using AdventOfCode.Utils;

namespace AdventOfCode.Solutions
{
  public abstract class CSharpSolution
  {
    public abstract void Solve(PuzzleInput input);
    protected static SolutionRunner.cDelSubmit SubmitPartOne = SolutionRunner.cDelOne;
    protected static SolutionRunner.cDelSubmit SubmitPartTwo = SolutionRunner.cDelTwo;
  }
}
