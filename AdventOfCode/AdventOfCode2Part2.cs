using System.IO;
using System.Linq;

namespace AdventOfCode;

public sealed class AdventOfCode2Part2
{
    public static int Run()
    {
        var totalScore = File.ReadLines("adventOfCode2Input.txt")
            .Select(ParseFromString)
            .Select(u => (My: ReversePlay(u.Outcome, u.Enemy), Outcome: u.Outcome))
            .Select(u => ScoreForSelectedShape(u.My) + ScoreForGameOutcome(u.Outcome))
            .Sum();

        return totalScore;
    }

    private static (GameOutcome Outcome, Shape Enemy) ParseFromString(string line)
    {
        var enemy = FromEnemyChoice(line[0]);
        var outcome = FromOutcome(line[2]);
        return (outcome, enemy);
    }

    private static Shape FromEnemyChoice(char c)
        => c switch
        {
            'A' => Shape.Rock,
            'B' => Shape.Paper,
            'C' => Shape.Scissors,
        };

    private static GameOutcome FromOutcome(char c)
        => c switch
        {
            'X' => GameOutcome.Lose,
            'Y' => GameOutcome.Draw,
            'Z' => GameOutcome.Win,
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

    private static Shape ReversePlay(GameOutcome outcome, Shape enemy)
        => (outcome, enemy) switch
        {
            (GameOutcome.Win, Shape.Scissors) => Shape.Rock,
            (GameOutcome.Win, Shape.Rock) => Shape.Paper,
            (GameOutcome.Win, Shape.Paper) => Shape.Scissors,
            (GameOutcome.Lose, Shape.Paper) => Shape.Rock,
            (GameOutcome.Lose, Shape.Scissors) => Shape.Paper,
            (GameOutcome.Lose, Shape.Rock) => Shape.Scissors,
            _ => enemy,
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