using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode;

public sealed class AdventOfCode8
{
    public static HashSet<(int, int)> Part1()
    {
        var trees = new List<List<TreeWithPosition>>();
        var lines = File.ReadLines("adventOfCode8Input.txt");
        var charOffset = '0' - 0;
        foreach (var (line, indexX) in lines.Select((u, index) => (Line: u, index)))
        {
            trees.Add(line.Select((u, indexY) => new TreeWithPosition
            {
                Height = u - charOffset,
                X = indexX,
                Y = indexY
            }).ToList());
        }

        var visibleTreesIndexes = new HashSet<(int, int)>(trees.Count * trees[0].Count);
            
        AddHorizontallyVisibleTrees(trees, visibleTreesIndexes);

        ChangeLineOfSight(trees);
        
        AddHorizontallyVisibleTrees(trees, visibleTreesIndexes);

        return visibleTreesIndexes;
    }
    
    public static int Part2()
    {
        var trees = InitializeTrees();

        for (int i = 0; i < trees.Count - 1; i++)
        {
            for (int j = 0; j < trees[i].Count - 1; j++)
            {
                FillVisibilityForDirection(trees, i, j, Direction.Left);
                FillVisibilityForDirection(trees, i, j, Direction.Up);
            }
        }

        for (int y = trees.Count - 1; y >= 0; y--)
        {
            for (int x = trees[y].Count - 1; x >= 0; x--)
            {
                FillVisibilityForDirection(trees, y, x, Direction.Right);
                FillVisibilityForDirection(trees, y, x, Direction.Down);
            }
        }

        var max = trees
            .SelectMany(u => u)
            .Aggregate(int.MinValue, (accumulated, tree) => Math.Max(accumulated, tree.ScenicScore));

        return max;
    }

    private static List<List<Tree>> InitializeTrees()
    {
        var lines = File.ReadLines("adventOfCode8Input.txt");
        var charOffset = '0' - 0;

        var trees = lines.Select(line => line
            .Select(u => new Tree { Height = u - charOffset }).ToList())
            .ToList();

        foreach (var tree in trees.First())
        {
            SetBorderViewLengths(tree);
        }

        foreach (var tree in trees.Last())
        {
            SetBorderViewLengths(tree);
        }

        foreach (var (first, last) in trees.Select(u => (u.First(), u.Last())))
        {
            SetBorderViewLengths(first);
            SetBorderViewLengths(last);
        }

        return trees;
    }

    private static void SetBorderViewLengths(Tree tree)
    {
        tree.MaxByDirection[Direction.Down] = 0;
        tree.MaxByDirection[Direction.Up] = 0;
        tree.MaxByDirection[Direction.Left] = 0;
        tree.MaxByDirection[Direction.Right] = 0;
    }

    private static void FillVisibilityForDirection(List<List<Tree>> trees, int y, int x, Direction direction)
    {
        var mainTree = trees[y][x];

        var (newY, newX) = AdvanceIndexes(trees, y, x, direction, 1);

        while (newY is not null && newX is not null)
        {
            var tree = trees[newY.Value][newX.Value];

            if (tree.Height >= mainTree.Height)
                return;

            mainTree.MaxByDirection[direction] += tree.MaxByDirection[direction];

            if (tree.MaxByDirection[direction] == 0)
                return;
            
            (newY, newX) = AdvanceIndexes(trees, newY.Value, newX.Value, direction, tree.MaxByDirection[direction]);
        }
    }

    private static (int? newY, int? newX) AdvanceIndexes(List<List<Tree>> trees, int y, int x, Direction direction, int count)
    {
        int? nullifiedX = x;
        int? nullifiedY = y;
        
        if (direction == Direction.Down)
        {
            y += count;
            nullifiedY = y switch
            {
                _ when y >= trees.Count => null,
                _ => y
            };
        }
        if (direction == Direction.Up)
        {
            y -= count;
            nullifiedY = y switch
            {
                < 0 => null,
                _ => y
            };
        }
        if (direction == Direction.Right)
        {
            x += count;
            nullifiedX = x switch
            {
                _ when x >= trees[y].Count => null,
                _ => x
            };
        }
        if (direction == Direction.Left)
        {
            x -= count;
            nullifiedX = x switch
            {
                < 0 => null,
                _ => x
            };
        }

        return (nullifiedY, nullifiedX);
    }

    private static void ChangeLineOfSight<T>(List<List<T>> trees)
    {
        for (int i = 0; i < trees.Count - 1; i++)
        {
            for (int j = i + 1; j < trees[i].Count; j++)
            {
                (trees[i][j], trees[j][i]) = (trees[j][i], trees[i][j]);
            }
        }
    }

    private static void AddHorizontallyVisibleTrees(List<List<TreeWithPosition>> trees, HashSet<(int, int)> visibleTreesIndexes)
    {
        var visibleTreesIndexesFromLeft = new HashSet<(int, int)>(trees.Count);
        var visibleTreesIndexesFromRight = new HashSet<(int, int)>(trees.Count);

        foreach (var (treeLineLeft, treeLineRight) in trees
            .Select(u => (TreeLineLeft: u, TreeLineRight: u.AsEnumerable().Reverse())))
        {
            var maxBeforeFromLeft = int.MinValue;
            var maxBeforeFromRight = int.MinValue;

            visibleTreesIndexesFromLeft.Clear();
            visibleTreesIndexesFromRight.Clear();

            foreach (var (leftTree, rightTree) in treeLineLeft
                .Zip(treeLineRight, (u, v) => (LeftTree: u, RightTree: v)))
            {
                if (maxBeforeFromLeft < leftTree.Height)
                {
                    maxBeforeFromLeft = leftTree.Height;
                    visibleTreesIndexesFromLeft.Add((leftTree.Y, leftTree.X));
                }

                if (maxBeforeFromRight < rightTree.Height)
                {
                    maxBeforeFromRight = rightTree.Height;
                    visibleTreesIndexesFromRight.Add((rightTree.Y, rightTree.X));
                }
            }

            visibleTreesIndexes.UnionWith(visibleTreesIndexesFromLeft);
            visibleTreesIndexes.UnionWith(visibleTreesIndexesFromRight);
        }
    }

    private class TreeWithPosition
    {
        public required int Height { get; init; }
        public required int X { get; init; }
        public required int Y { get; init; }
    }

    private class Tree
    {
        public required int Height { get; init; }

        public Dictionary<Direction, int> MaxByDirection { get; } = new()
        {
            { Direction.Down, 1 },
            { Direction.Up, 1 },
            { Direction.Left, 1 },
            { Direction.Right, 1 },
        };

        public int ScenicScore => MaxByDirection
            .Select(u => u.Value)
            .Aggregate(1, (accumulated, current) => accumulated * current);
    }

    private enum Direction
    {
        Up = 0,
        Down = 1,
        Left = 2,
        Right = 3
    }
}