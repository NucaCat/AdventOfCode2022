using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode;

public sealed class AdventOfCode6Part1
{
    public static int Run()
    {
        var bufferSize = 14;
        
        var buffer = File.ReadLines("adventOfCode6Input.txt").First();
        var queue = new Queue<char>(buffer.Take(bufferSize));
        var index = bufferSize;
        
        foreach (var character in buffer.Skip(bufferSize))
        {
            if (queue.Distinct().Count() == bufferSize)
                return index;
            
            queue.Dequeue();
            queue.Enqueue(character);
            index++;
        }
        
        throw new Exception();
    }
}