namespace AdventCode.Quatro;

internal class Assignment
{
    private readonly string _assignment;
    public Assignment(string assignment) => _assignment = assignment;
    public int StartingRange => int.Parse(_assignment.Split("-").First());
    public int EndingRange => int.Parse(_assignment.Split("-").Last());
    public IList<int> Sections => Enumerable.Range(StartingRange, EndingRange - StartingRange + 1).ToList();
}

internal class Pair
{
    public Pair(string pairs) => Assignments = pairs.Split(",").Select(s => new Assignment(s)).ToList();
    public IList<Assignment> Assignments { get; }
    public bool AssignmentsOverlap => ContainsOverlap(Assignments.First(), Assignments.Last());
    public bool AnyOverlap => Assignments.First().Sections.Intersect(Assignments.Last().Sections).Any();
    private bool ContainsOverlap(Assignment initial, Assignment second)
    {
        var intersection = initial.Sections.Intersect(second.Sections).ToList();
        var containsOverlap = intersection.Count == initial.Sections.Count ||
                              intersection.Count == second.Sections.Count;
        return containsOverlap;
    }
}

public class DayFour
{
    public static void Run()
    {
        Console.WriteLine("******");
        Console.WriteLine("Day 4");
        var lines = File.ReadAllLines("Quatro/Assignments.txt");
        var pairs = lines.Select(sackLine => new Pair(sackLine)).ToList();
        var totalOverlaps = pairs.Count(s => s.AssignmentsOverlap);
        var anyOverLap = pairs.Count(s => s.AnyOverlap);
        Console.WriteLine($"Total Overlap: {totalOverlaps}");
        Console.WriteLine($"Any Overlap: {anyOverLap}");
    }
}