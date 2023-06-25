using AdventOfCode;
using NUnit.Framework;

namespace Tests;

[TestFixture]
public class Tests
{
    [Test]
    public void Aoc8Part1()
    {
        var actual = AdventOfCode8.Part1();
        var expected = new HashSet<(int, int)>(new List<(int, int)>(new []
        {
            (0, 0), (1, 0), (2, 0), (3, 0), (4, 0),
            (0, 1), (1, 1), (2, 1),         (4, 1),
            (0, 2), (1, 2),         (3, 2), (4, 2),
            (0, 3),         (2, 3),         (4, 3),
            (0, 4), (1, 4), (2, 4), (3, 4), (4, 4),
        }));

        Assert.IsTrue(expected.IsSubsetOf(actual));
        Assert.IsTrue(actual.IsSubsetOf(expected));
    }
    [Test]
    public void Aoc8Part2()
    {
        var actual = AdventOfCode8.Part2();
        var expected = 157320;
        Assert.AreEqual(actual, expected);
    }
    [Test]
    public void Aoc9Part1()
    {
        var actual = AdventOfCode9.Part1();

        Assert.AreEqual(actual.Count, 6067);
    }
    [Test]
    public void Aoc9Part2()
    {
        var actual = AdventOfCode9.Part2();

        Assert.AreEqual(actual.Count, 2471);
    }
}

