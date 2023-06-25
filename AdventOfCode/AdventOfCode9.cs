using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode;

public sealed class AdventOfCode9
{
    public static HashSet<(int, int)> Part1()
    {
        var moves = File.ReadLines("adventOfCode9Input.txt")
            .Select(Move.FromString)
            .ToList();

        var tail = new Rope { X = 0, Y = 0 };
        var head = new Rope { X = 0, Y = 0 };

        var visitedByTail = new HashSet<(int, int)>(moves.Count) { tail.ToTuple() };

        foreach (var move in moves
            .SelectMany(u => Enumerable.Range(0, u.MovesCount).Select(_ => u.Direction)))
        {
            MoveHead(head, move);
            MoveTail(tail, head);

            visitedByTail.Add(tail.ToTuple());
        }

        return visitedByTail;
    }
    public static HashSet<(int, int)> Part2()
    {
        var moves = File.ReadLines("adventOfCode9Input.txt")
            .Select(Move.FromString)
            .ToList();

        const int knotCount = 9;
        
        var longTailWithHead = Enumerable.Range(0, knotCount + 1)
            .Select(_ => new Rope { X = 0, Y = 0 })
            .ToList();

        var head = longTailWithHead.First();
        var tail = longTailWithHead.Last();
        
        var zipped = longTailWithHead.Take(longTailWithHead.Count - 1)
            .Zip(longTailWithHead.Skip(1), (u, v) => (Head: u, Tail: v))
            .ToList();

        var visitedByTail = new HashSet<(int, int)>(moves.Count) { (0, 0) };

        foreach (var move in moves
            .SelectMany(u => Enumerable.Range(0, u.MovesCount).Select(_ => u.Direction)))
        {
            MoveHead(head, move);
            
            foreach (var (currentHead, currentTail) in zipped)
            {
                MoveTail(currentTail, currentHead);
            }
            
            visitedByTail.Add(tail.ToTuple());
        }

        return visitedByTail;
    }

    private static void MoveTail(Rope tail, Rope head)
    {
        var xOffset = head.X - tail.X;
        var yOffset = head.Y - tail.Y;

        var moveHorizontally = Math.Abs(xOffset) > 1;
        var moveVertically = Math.Abs(yOffset) > 1;

        var moveDiagonally = (moveHorizontally && yOffset != 0) || (moveVertically && xOffset != 0);

        var _ = (moveHorizontally, moveVertically, moveDiagonally) switch
        {
            (true, _, false) => tail.MoveHorizontally(xOffset > 0 ? 1 : -1),
            (_, true, false) => tail.MoveVertically(yOffset > 0 ? 1 : -1),
            (_, _, true) => tail.MoveVertically(yOffset > 0 ? 1 : -1).MoveHorizontally(xOffset > 0 ? 1 : -1),
            _ => tail
        };
    }

    private static void MoveHead(Rope head, Direction direction)
    {
        head.X += direction switch
        {
            Direction.Left => -1,
            Direction.Right => 1,
            _ => 0
        };
        head.Y += direction switch
        {
            Direction.Up => -1,
            Direction.Down => 1,
            _ => 0
        };
    }

    private class Move
    {
        public required Direction Direction { get; init; }
        public required int MovesCount { get; init; }

        public static Move FromString(string str)
        {
            var tokens = str.Split(" ");
            var move = new Move
            {
                Direction = tokens[0] switch
                {
                    "R" => Direction.Right,
                    "L" => Direction.Left,
                    "U" => Direction.Up,
                    "D" => Direction.Down,
                    _ => throw new ArgumentOutOfRangeException()
                },
                MovesCount = Convert.ToInt32(tokens[1]),
            };
            return move;
        }
    }

    private enum Direction
    {
        Up, 
        Down,
        Left,
        Right
    }
    
    private class Rope
    {
        public required int X { get; set; }
        public required int Y { get; set; }

        public (int, int) ToTuple() => (X, Y);
        
        public Rope MoveHorizontally(int xOffset)
        {
            X += xOffset;
            return this;
        }
        
        public Rope MoveVertically(int yOffset)
        {
            Y += yOffset;
            return this;
        }
    }
}