namespace AdventCode.Dos;
enum Choice
{
    Rock = 1,
    Paper = 2,
    Scissors = 3
}

enum Outcome
{
    Lose = 0,
    Draw = 3,
    Win = 6
}

internal class Round
{
    private Round(Choice elf, Choice mine, Outcome desiredOutcome)
    {
        Elf = elf;
        Mine = mine;
        DesiredOutcome = desiredOutcome;
    }

    public Choice Elf { get; }
    public Choice Mine { get; }
    public Outcome DesiredOutcome { get; }
    public Choice DesiredChoice => GetChoiceBasedOnOutCome();
    public int Score() => (int)PlayRound(Elf,Mine) + (int)Mine;
    public int Score2() => (int)PlayRound(Elf, DesiredChoice) + (int)DesiredChoice;

    public static Outcome PlayRound(Choice elf, Choice mine)
    {
        if (elf == mine)
            return Outcome.Draw;
        return elf switch
        {
            Choice.Rock => mine == Choice.Paper ? Outcome.Win : Outcome.Lose,
            Choice.Paper => mine == Choice.Scissors ? Outcome.Win : Outcome.Lose,
            Choice.Scissors => mine == Choice.Rock ? Outcome.Win : Outcome.Lose,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public Choice GetChoiceBasedOnOutCome()
    {
        if (DesiredOutcome == Outcome.Draw)
            return Elf;
        return Elf switch
        {
            Choice.Rock => DesiredOutcome == Outcome.Win ? Choice.Paper : Choice.Scissors,
            Choice.Paper => DesiredOutcome == Outcome.Win ? Choice.Scissors : Choice.Rock,
            Choice.Scissors => DesiredOutcome == Outcome.Win ? Choice.Rock : Choice.Paper,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static Outcome GetOutcome(string value)
    {
        return value.ToLower() switch
        {
            "x" => Outcome.Lose,
            "y" => Outcome.Draw,
            "z" => Outcome.Win,
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    private static Choice GetChoice(string value)
    {
        return value.ToLower() switch
        {
            "a" or "x" => Choice.Rock,
            "b" or "y" => Choice.Paper,
            "c" or "z" => Choice.Scissors,
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    public static Round CreateRound(string round)
    {
        var choices = round.Split(" ");
        var choice = GetChoice(choices.First());
        var myChoice = GetChoice(choices.Last());
        var desiredOutCome = GetOutcome(choices.Last());
        return new Round(choice, myChoice, desiredOutCome);
    }
}

public class DayTwo
{
    public static void Run()
    {
        var lines = File.ReadAllLines("Dos/Rounds.txt");
        var rounds = lines.Select(Round.CreateRound).ToList();
        var totalScore = rounds.Select(s => s.Score()).Sum();
        var totalScore2 = rounds.Select(s => s.Score2()).Sum();
        Console.WriteLine("******");
        Console.WriteLine("Day Two!");
        Console.WriteLine($"Total Score: {totalScore}");
        Console.WriteLine($"Real Total Score: {totalScore2}");
    }
}