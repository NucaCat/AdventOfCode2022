using System.IO;
using System.Linq;

namespace AdventOfCode;

public sealed class AdventOfCode2Part1
{
    public static int Run()
    {
        var totalScore = File.ReadLines("adventOfCode2Input.txt")
            .Select(ParseFromString)
            .Select(u => (My: u.My, Outcome: Play(my: u.My, enemy: u.Enemy)))
            .Select(u => ScoreForSelectedShape(u.My) + ScoreForGameOutcome(u.Outcome))
            .Sum();

        return totalScore;
    }

    private static (Shape My, Shape Enemy) ParseFromString(string line)
    {
        var enemy = FromEnemyChoice(line[0]);
        var my = FromMyChoice(line[2]);
        return (my, enemy);
    }

    private static Shape FromEnemyChoice(char c)
        => c switch
        {
            'A' => Shape.Rock,
            'B' => Shape.Paper,
            'C' => Shape.Scissors,
        };

    private static Shape FromMyChoice(char c)
        => c switch
        {
            'X' => Shape.Rock,
            'Y' => Shape.Paper,
            'Z' => Shape.Scissors,
        };

    private static int ScoreForSelectedShape(Shape shape)
        => shape switch
        {
            Shape.Rock => 1,
            Shape.Paper => 2,
            Shape.Scissors => 3,
        };

    private static int ScoreForGameOutcome(GameOutcome outcome)
        => outcome switch
        {
            GameOutcome.Lose => 0,
            GameOutcome.Draw => 3,
            GameOutcome.Win => 6,
        };

    private static GameOutcome Play(Shape my, Shape enemy)
        => (my, enemy) switch
        {
            (Shape.Rock, Shape.Scissors) => GameOutcome.Win,
            (Shape.Rock, Shape.Paper) => GameOutcome.Lose,
            (Shape.Paper, Shape.Rock) => GameOutcome.Win,
            (Shape.Paper, Shape.Scissors) => GameOutcome.Lose,
            (Shape.Scissors, Shape.Paper) => GameOutcome.Win,
            (Shape.Scissors, Shape.Rock) => GameOutcome.Lose,
            _ => GameOutcome.Draw,
        };
    
    private enum Shape
    {
        Rock,
        Paper,
        Scissors,
    }
    
    private enum GameOutcome
    {
        Win,
        Lose,
        Draw
    }
}