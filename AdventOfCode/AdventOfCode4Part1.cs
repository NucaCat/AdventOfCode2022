using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode;

public sealed class AdventOfCode4Part1
{
    public static int Run()
    {
        var prioritiesSum = File.ReadLines("adventOfCode4Input.txt")
            .Select(ParsePairs)
            .Count(u => IsFullyContainedByTheOther(u.FirstRange, u.SecondRange));
        
        return prioritiesSum;
    }

    private static bool IsFullyContainedByTheOther(Range first, Range second)
        => SecondIsContainedInTheFirst(first, second)
            || SecondIsContainedInTheFirst(second, first);

    private static bool SecondIsContainedInTheFirst(Range first, Range second)
    {
        return first.LowerBound <= second.LowerBound && first.UpperBound >= second.UpperBound;
    }

    private static (Range FirstRange, Range SecondRange) ParsePairs(string line)
    {
        var parts = line.Split(',');
        var firstRange = ParseToRange(parts[0]);
        var secondRange = ParseToRange(parts[1]);

        return (firstRange, secondRange);
    }

    private static Range ParseToRange(string stringRange)
    {
        var parts = stringRange.Split('-');
        var range = new Range
        {
            LowerBound = Convert.ToInt32(parts[0]),
            UpperBound = Convert.ToInt32(parts[1]),
        };
        return range;
    }

    private class Range
    {
        public int LowerBound { get; set; }
        public int UpperBound { get; set; }
    }
}