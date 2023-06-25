using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode;

public sealed class AdventOfCode3Part1
{
    private static readonly int _lowerStartPriority = 1;
    private static readonly int _upperStartPriority = 27;

    public static int Run()
    {
        var prioritiesSum = File.ReadLines("adventOfCode3Input.txt")
            .Select(FindIntersectingCharacter)
            .Select(ToPriority)
            .Sum();
        
        return prioritiesSum;
    }

    private static char FindIntersectingCharacter(string s)
    {
        var firstHalf = new HashSet<char>(s.Length / 2);
        var secondHalf = new HashSet<char>(s.Length / 2);

        var offset = s.Length / 2;
        
        for (int i = 0; i < s.Length / 2; i++)
        {
            firstHalf.Add(s[i]);
            secondHalf.Add(s[i + offset]);
        }

        var intersectingCharacter = firstHalf.First(u => secondHalf.Contains(u));
        return intersectingCharacter;
    }

    private static int ToPriority(char c)
        => IsUpperCase(c) ? c - 'A' + _upperStartPriority : c - 'a' + _lowerStartPriority ;

    private static bool IsUpperCase(char c)
        => c >= 'A' && c <= 'Z';
}