# Advent Of Code
This repository provides resources for creating Advent of Code puzzle solutions. It requires .NET 5.0 on Windows 10 to access all functionality. 
Currently only supports solutions written in C#.

To get started, open the repository in VS Code, then run `dotnet build` to install dependencies.

## Setting up files
To create the folder structure for a new year, run `dotnet run --y YYYY --init`, where YYYY is the year you'd like to initialize. This will create a new folder under `src/Puzzles` for the given year, with subfolders for the puzzle inputs and solutions.

## Downloading inputs
Inputs are stored in the `Inputs` subdirectory for each year. When a test is run, the application will check the directory so see if the given input exists. If not, it will be downloaded automatically, so long as the user has put their session cookie information under `"cookie":` in the `config.json` file. Downloading inputs for dates in the future will not be attempted.

## Writing tests
Each test template contains a method or function named `solve`. This is the entry point for your solution. C# solutions have a `PuzzleInput` object that contains extra methods for parsing the input into different common puzzle structures.

Solutions are reported with the functions `SubmitPartOne` and `SubmitPartTwo`. Using these functions will store your answer and record the time the answer was found. 

## Running tests
Tests can be run with `dotnet run --y YYYY --d DD`. Omitting the day argument will attempt to run all tests for the given year. The results will be displayed for each part, along with the milliseconds after starting the test that each part of the solution was reported.

If an argument is missing, the program will check the `config.json` file to see if a default has been set. This can be useful if you want to use the vscode debugger.
