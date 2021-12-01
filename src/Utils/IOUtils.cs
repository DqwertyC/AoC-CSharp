using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AdventOfCode.Utils
{
  static class IOUtils
  {
    private static string _rootDir = Directory.GetCurrentDirectory();
    private static bool _rootDirFound = false;

    private static JObject _config;
    private static bool _configFound = false;

    public static void PrintUsage(this TextWriter console)
    {
      console.WriteLine("Usage\n" +
                        "\t--h, --help:\n\t\tDisplay this usage message\n" +
                        "\t--y, --year [year]:\n\t\tSet the year to test or create a folder for. Must be at least 2015.\n" +
                        "\t--d, --day [day]:\n\t\tSet the day to test. Must be between 1 and 25 (inclusive).\n" +
                        "\t--init:\n\t\tCreate a new folder structure for the given year\n"
                        );
    }

    public static void PrintSolution(this TextWriter console, (string solution, double millis) partOne, (string solution, double millis) partTwo)
    {
      console.WriteLine($"Part One: ({partOne.millis} ticks)\n{partOne.solution}\n\nPart Two: ({partTwo.millis} ticks)\n{partTwo.solution}");
    }

    public static JObject ConfigObject()
    {
      if (!_configFound)
      {
        if (!_rootDirFound)
          FindRootDir();

        string configPath = Path.Combine(_rootDir, $"../config.json");
        _config = (JObject)JToken.ReadFrom(new JsonTextReader(File.OpenText(configPath)));
        _configFound = true;
      }

      return _config;
    }

    public static string TemplatePath()
    {
      if (!_rootDirFound) FindRootDir();

      return Path.Combine(_rootDir, $"Templates/CSolutionTemplate.txt");
    }

    public static string SolutionPath(int year, int day)
    {
      return CSolutionPath(year, day);
    }

    public static string SolutionPath(int year)
    {
      return CSolutionPath(year);
    }

    public static string InputPath(int year, int day)
    {
      if (!_rootDirFound) FindRootDir();
      return Path.Combine(_rootDir, $"Puzzles/{year}/Inputs/Input_{day:D2}");
    }

    public static string InputPath(int year)
    {
      if (!_rootDirFound) FindRootDir();
      return Path.Combine(_rootDir, $"Puzzles/{year}/Inputs");
    }

    public static string CSolutionPath(int year, int day)
    {
      if (!_rootDirFound) FindRootDir();
      return Path.Combine(_rootDir, $"Puzzles/{year}/CSharp/Solution_{year}_{day:D2}.cs");
    }

    public static string CSolutionPath(int year)
    {
      if (!_rootDirFound) FindRootDir();
      return Path.Combine(_rootDir, $"Puzzles/{year}/CSharp");
    }

    public static string JSolutionPath(int year, int day)
    {
      if (!_rootDirFound) FindRootDir();
      return Path.Combine(_rootDir, $"Puzzles/{year}/JavaScript/Solution_{year}_{day:D2}.js");
    }

    public static string JSolutionPath(int year)
    {
      if (!_rootDirFound) FindRootDir();
      return Path.Combine(_rootDir, $"Puzzles/{year}/JavaScript");
    }

    public static string PSolutionPath(int year, int day)
    {
      if (!_rootDirFound) FindRootDir();
      return Path.Combine(_rootDir, $"Puzzles/{year}/Python/Solution_{year}_{day:D2}.py");
    }

    public static string PSolutionPath(int year)
    {
      if (!_rootDirFound) FindRootDir();
      return Path.Combine(_rootDir, $"Puzzles/{year}/Python");
    }

    public static string InputURL(int year, int day)
    {
      return $"https://adventofcode.com/{year}/day/{day}/input";
    }

    private static void FindRootDir()
    {
      while (!Directory.Exists(Path.Combine(_rootDir, "src")))
      {
        _rootDir = Path.Combine(_rootDir, "..");
      }
      _rootDir = Path.Combine(_rootDir, "src");
      _rootDirFound = true;
    }
  }
}
