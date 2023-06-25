using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode;

public sealed class AdventOfCode5
{
    private static Dictionary<int, Stack<char>> _cratesWithStack = new();
    private static readonly Dictionary<int, List<char>> _cratesWithList = new();

    private static bool _isMoves = false;

    public static string Part1()
    {
        var lines = File.ReadLines("adventOfCode5Input.txt");
        
        foreach (var line in lines)
        {
            if (line == string.Empty)
            {
                MoveToCratesWithStack();
                continue;
            }

            if (!_isMoves)
                ParseCrateAndAddToList(line);

            if (_isMoves)
                MoveCratesOneByOne(line);
        }

        var result = CollectTopCrates();
        return result;
    }

    public static string Part2()
    {
        var lines = File.ReadLines("adventOfCode5Input.txt");
        
        foreach (var line in lines)
        {
            if (line == string.Empty)
            {
                MoveToCratesWithStack();
                continue;
            }

            if (!_isMoves)
                ParseCrateAndAddToList(line);

            if (_isMoves)
                MoveCratesBatch(line);
        }

        var result = CollectTopCrates();
        return result;
    }

    private static string CollectTopCrates()
    {
        var resultChars = _cratesWithStack
            .OrderBy(u => u.Key)
            .Select(u =>
            {
                if (u.Value.TryPop(out var crate))
                    return (char?)crate;
                return null;
            })
            .Where(u => u.HasValue)
            .Select(u => u.Value)
            .ToArray();
        var result = new string(resultChars);
        return result;
    }

    private static void MoveCratesOneByOne(string line)
    {
        var move = ParseMoveFromString(line);

        var source = _cratesWithStack[move.Source];
        var destination = _cratesWithStack[move.Destination];

        for (int i = 0; i < move.Count; i++)
        {
            destination.Push(source.Pop());
        }
    }

    private static void MoveCratesBatch(string line)
    {
        var move = ParseMoveFromString(line);

        var source = _cratesWithStack[move.Source];
        var destination = _cratesWithStack[move.Destination];

        var tempStack = new Stack<char>(move.Count);
        for (int i = 0; i < move.Count; i++)
        {
            tempStack.Push(source.Pop());
        }

        for (int i = 0; i < move.Count; i++)
        {
            destination.Push(tempStack.Pop());
        }
    }

    private static Move ParseMoveFromString(string line)
    {
        var move = new Move();

        var indexOfMove = IndexOfSubstring(line, "move ");  
        var indexOfFrom = IndexOfSubstring(line, "from ");  
        var indexOfTo = IndexOfSubstring(line, "to ");

        move.Count = Convert.ToInt32(line.Substring(indexOfMove.End, indexOfFrom.Start - indexOfMove.End));
        move.Source = Convert.ToInt32(line.Substring(indexOfFrom.End, indexOfTo.Start - indexOfFrom.End));
        move.Destination = Convert.ToInt32(line.Substring(indexOfTo.End));

        return move;
    }

    private static (int Start, int End) IndexOfSubstring(string line, string substring)
    {
        var start = line.IndexOf(substring);
        var end = start + substring.Length;
        return (start, end);
    }

    private class Move
    {
        public int Count { get; set; }
        public int Source { get; set; }
        public int Destination { get; set; }
    }

    private static void ParseCrateAndAddToList(string line)
    {
        if (line.All(u => u == ']'))
            return;

        var parsedCrates = ParseCrateFromString(line);

        foreach (var parsedCrate in parsedCrates)
        {
            if (!_cratesWithList.TryGetValue(parsedCrate.Column, out var list))
            {
                list = new List<char>();
                _cratesWithList[parsedCrate.Column] = list;
            }

            list.Add(parsedCrate.Crate);
        }
    }

    private static void MoveToCratesWithStack()
    {
        _cratesWithStack = _cratesWithList
            .ToDictionary(u => u.Key, u =>
            {
                u.Value.Reverse();
                return new Stack<char>(u.Value);
            });

        _isMoves = true;
    }

    private static IEnumerable<(int Column, char Crate)> ParseCrateFromString(string line)
        => line
            .Chunk(4)
            .Select(u => u.Take(3))
            .Select((u, i) => (Column: i + 1, Crate: u.First(v => v != ']' && v != '[')))
            .Where(u => u.Crate != ' ');
}