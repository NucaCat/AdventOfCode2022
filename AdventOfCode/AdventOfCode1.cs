using System;
using System.IO;
using System.Linq;

namespace AdventOfCode;

public sealed class AdventOfCode1
{
    public static int Run()
    {
        var maxTopThree = new[]
        {
            int.MinValue,
            int.MinValue,
            int.MinValue
        };
        var currentMax = 0;

        var lines = File.ReadLines("adventOfCode1Input.txt");
        foreach (var line in lines)
        {
            if (line == string.Empty)
            {
                currentMax += Convert.ToInt32(line);
                continue;
            }

            for (int i = 0; i < maxTopThree.Length; i++)
            {
                if (currentMax > maxTopThree[i])
                {
                    maxTopThree[i] = currentMax;
                    Array.Sort(maxTopThree);
                    break;
                }
            }

            currentMax = 0;
        }

        return maxTopThree.Sum();
    }
}