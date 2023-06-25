using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode;

public sealed class AdventOfCode7
{
    private static readonly Directory _baseDirectory = new();
    private static Directory _currentDirectory = _baseDirectory;
    private const int _sizeThreshold = 100_000;
    private const int _totalDiscSpace = 70_000_000;
    private const int _spaceNeeded = 30_000_000;

    public static int Part1()
    {
        ParseAndExecuteCommands();
        var allSums = SizesOfInnerDirectories(_baseDirectory);

        var sumsWithThreshold = allSums.Where(u => u <= _sizeThreshold);

        var totalSum = sumsWithThreshold.Sum();

        return totalSum;
    }

    public static int Part2()
    {
        ParseAndExecuteCommands();
        var allSums = SizesOfInnerDirectories(_baseDirectory).ToList();
        
        var unusedSpace = _totalDiscSpace - allSums.Max();
        var threshold = _spaceNeeded - unusedSpace;

        var sumsWithThreshold = allSums.Where(u => u >= threshold);

        var totalSum = sumsWithThreshold.Min();

        return totalSum;
    }

    private static IEnumerable<int> SizesOfInnerDirectories(Directory directory)
    {
        var sumOfFilesInDirectory = directory.Files.Select(u => u.Size).Sum();
        if (!directory.Directories.Any())
            return new []{ sumOfFilesInDirectory };

        var sizesOfDirectories = directory.Directories.Select(SizesOfInnerDirectories).ToList();

        var sums = sizesOfDirectories.SelectMany(u => u).ToList();
        var sums1 = sizesOfDirectories.Select(u => u.Max());
        sums.Add(sums1.Sum() + sumOfFilesInDirectory);

        return sums;
    }

    private static void ParseAndExecuteCommands()
    {
        var lines = File.ReadLines("adventOfCode7Input.txt");
        foreach (var line in lines)
        {
            var command = ParseCommand(line);
            if (command.Command == Commands.SwitchToBaseDirectory)
                _currentDirectory = _baseDirectory;

            if (command.Command == Commands.SwitchToParentDirectory)
                _currentDirectory = _currentDirectory.ParentDirectory ?? _baseDirectory;

            if (command.Command == Commands.SwitchToInnerDirectory)
            {
                var possibleDirectory = _currentDirectory.Directories.FirstOrDefault(u => u.Title == command.DirectoryTitle);
                if (possibleDirectory == null)
                {
                    possibleDirectory = new Directory { Title = command.DirectoryTitle, ParentDirectory = _currentDirectory };
                    _currentDirectory.Directories.Add(possibleDirectory);
                }

                _currentDirectory = possibleDirectory;
            }

            if (command.Command == Commands.AddFile)
            {
                var file = _currentDirectory.Files.FirstOrDefault(u => u.Title == command.File.Title);
                if (file == null)
                    _currentDirectory.Files.Add(command.File);
            }

            if (command.Command == Commands.AddDirectory)
            {
                var possibleDirectory = _currentDirectory.Directories.FirstOrDefault(u => u.Title == command.DirectoryTitle);
                if (possibleDirectory == null)
                {
                    possibleDirectory = new Directory { Title = command.DirectoryTitle, ParentDirectory = _currentDirectory };
                    _currentDirectory.Directories.Add(possibleDirectory);
                }
            }
        }
    }

    private static CommandWithParams ParseCommand(string line)
    {
        if (line.StartsWith("$ cd /"))
            return new CommandWithParams { Command = Commands.SwitchToBaseDirectory };

        if (line.StartsWith("$ cd .."))
            return new CommandWithParams { Command = Commands.SwitchToParentDirectory };

        if (line.StartsWith("$ cd"))
            return new CommandWithParams
            {
                Command = Commands.SwitchToInnerDirectory,
                DirectoryTitle = line.Substring("$ cd".Length + 1)
            };

        if (line.StartsWith("$ ls"))
            return new CommandWithParams { Command = Commands.PrintList };

        if (line.StartsWith("dir"))
            return new CommandWithParams
            {
                Command = Commands.AddDirectory,
                DirectoryTitle = line.Substring("dir".Length + 1)
            };

        var lines = line.Split(" ");

        return new CommandWithParams
        {
            Command = Commands.AddFile,
            File = new MyFile { Size = Convert.ToInt32(lines[0]), Title = lines[1] }
        };
    }

    private class CommandWithParams
    {
        public Commands Command { get; set; }
        public string DirectoryTitle { get; set; }
        public MyFile File { get; set; }
    }

    private enum Commands
    {
        SwitchToBaseDirectory,
        SwitchToInnerDirectory,
        SwitchToParentDirectory,
        PrintList,
        AddDirectory,
        AddFile
    }

    private class Directory
    {
        public string Title { get; set; }
        public Directory? ParentDirectory { get; set; }
        public List<Directory> Directories { get; set; } = new();
        public List<MyFile> Files { get; set; } = new();
    }
    
    private class MyFile
    {
        public string Title { get; set; }
        public int Size { get; set; }
    }
}