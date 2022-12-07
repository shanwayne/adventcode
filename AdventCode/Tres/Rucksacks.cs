namespace AdventCode.Tres;
internal class Rucksack
{
    private readonly string _items;
    public Rucksack(string items)
    {
        _items = items;
    }

    public IEnumerable<char> Items => _items;
    public IEnumerable<char> CompartmentOne => _items.Take(_items.Length / 2);
    public IEnumerable<char> CompartmentTwo => _items.Skip(_items.Length / 2);

    public IEnumerable<char> MatchingItems => CompartmentOne.Intersect(CompartmentTwo);
    public int Sum => MatchingItems.Select(ConvertToPriority).Sum();
    public static int ConvertToPriority(char item)
    {
        var modifier = char.IsUpper(item) ? 26 : 0;
        var priority = (char.ToUpper(item) - 64) + modifier;
        return priority;
    }
}

internal class ElfGroup
{
    private readonly IList<Rucksack> _sacks = new List<Rucksack>();
    public void Add(Rucksack sack) => _sacks.Add(sack);
    public IEnumerable<char> MatchingItems => _sacks
        .Skip(1)
        .Aggregate(new HashSet<char>(_sacks.First().Items), (set, sack) =>
        {
            set.IntersectWith(sack.Items);
            return set;
        });

    public int Sum => MatchingItems.Select(Rucksack.ConvertToPriority).Sum();
        
}
public class DayThree
{
    public static void Run()
    {
        Console.WriteLine("******");
        Console.WriteLine("Day 3");
        var lines = File.ReadAllLines("Tres/Sacks.txt");
        var sacks = lines.Select(sackLine => new Rucksack(sackLine)).ToList();
        var sum = sacks.Select(s => s.Sum).Sum();
        ElfGroup currentGroup = new ElfGroup();
        var groups = new List<ElfGroup>();
        for (int i = 0; i < sacks.Count; i++)
        {
            if (i != 0 && i % 3 == 0)
            {
                groups.Add(currentGroup);
                currentGroup = new ElfGroup();
            }
            currentGroup.Add(sacks[i]);
        }
        groups.Add(currentGroup);
        var groupSum = groups.Select(s => s.Sum).Sum();
        Console.WriteLine($"Part 1: {sum}");
        Console.WriteLine($"Part 2: {groupSum}");
    }
}