using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode;

public sealed class AdventOfCode3Part2
{
    private static readonly int _lowerStartPriority = 1;
    private static readonly int _upperStartPriority = 27;

    public static int Run()
    {
        var prioritiesSum = File.ReadLines("adventOfCode3Input.txt")
            .Chunk(3)
            .Select(FindIntersectingCharacter)
            .Select(ToPriority)
            .Sum();
        
        return prioritiesSum;
    }

    private static char FindIntersectingCharacter(string[] s)
    {
        var first = s[0];
        var second = s[1];
        var third = s[2];

        var intersectingCharacter = first.First(u => second.Contains(u) && third.Contains(u));
        return intersectingCharacter;
    }

    private static int ToPriority(char c)
        => IsUpperCase(c) ? c - 'A' + _upperStartPriority : c - 'a' + _lowerStartPriority ;

    private static bool IsUpperCase(char c)
        => c >= 'A' && c <= 'Z';
}