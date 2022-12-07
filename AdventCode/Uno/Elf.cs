using static System.Console;

namespace AdventCode.Uno;
internal class Elf
{
    private readonly IList<int> _calories = new List<int>();
    public Elf(int id) => Id = id;
    public int Id { get; }
    public int TotalCalories => _calories.Sum();
    public void Add(int calories) => _calories.Add(calories);
}

public class DayOne
{
    public static void Run()
    {
        WriteLine("******");
        WriteLine("Day 1");
        var elves = new List<Elf>();
        var lines = File.ReadAllLines("Uno/CalorieList.txt");
        var currentElf = new Elf(elves.Count);
        foreach (var calories in lines)
        {
            if (string.IsNullOrWhiteSpace(calories))
            {
                elves.Add(currentElf);
                currentElf = new Elf(elves.Count);
                continue;
            }

            currentElf.Add(int.Parse(calories));
        }

        elves.Add(currentElf);

        var mostCalories = elves.OrderByDescending(s => s.TotalCalories).First();
        var top3 = elves.OrderByDescending(s => s.TotalCalories).Take(3);
        WriteLine($"Elf {mostCalories.Id} - {mostCalories.TotalCalories}");
        WriteLine($"Top 3 Elves  {string.Join(",", top3.Select(s => s.Id))} - Total Calories: {top3.Select(s => s.TotalCalories).Sum()} ");
    }
}