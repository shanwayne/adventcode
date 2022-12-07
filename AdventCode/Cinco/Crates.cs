using System.Text.RegularExpressions;

namespace AdventCode.Cinco;

internal class Crate
{
    private readonly string _crate;
    private const string Expression = "(\\[\\w\\])";
    public Crate(string crate)
    {
        _crate = crate;
    }
    public override string ToString() => _crate[1].ToString();

    public static Crate? Create(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
            return null;
        var matches = Regex.Match(input, Expression);
        if (!matches.Success)
            return null;
        var crate = new Crate(input);
        return crate;
        
    }
}

internal class CargoStack : Stack<Crate>
{
    public CargoStack(int number) => Number = number;
    public CargoStack(int number, CargoStack stack) : base(stack.Reverse()) => Number = number;
    public int Number { get; }

    public void AddCrate(Crate? crate)
    {
        if (crate == null)
            return;
        Push(crate);
    }

    public CargoStack Clone() => new(Number,this);
}

internal class Instruction
{
    private const string Expression = "\\w*\\s(\\d*)\\s*\\w*\\s(\\d*)\\s\\w*\\s(\\d*)";
    private readonly string _instruction;

    private Instruction(int quantity, int start, int end, string instruction)
    {
        _instruction = instruction;
        Quantity = quantity;
        Start = start;
        End = end;
    }

    public int Quantity { get; }
    public int Start { get; }
    public int End { get; }

    public void Execute(List<CargoStack> stacks, bool maintainOrder)
    {
        var items = new List<Crate>();
        for (var i = 0; i < Quantity; i++)
        {
            items.Add(stacks[Start-1].Pop());
            
        }

        if (maintainOrder)
            items.Reverse();

        foreach (var crate in items)
        {
            stacks[End - 1].Push(crate);
        }
    }
    public static Instruction Create(string instruction)
    {
        var matches = Regex.Match(instruction, Expression);
        if (!matches.Success)
            throw new ArgumentException($"Invalid Instruction: {instruction}");
        var quantity = int.Parse(matches.Groups[1].Value);
        var start = int.Parse(matches.Groups[2].Value);
        var end = int.Parse(matches.Groups[3].Value);
        return new Instruction(quantity, start, end, instruction);
    }
}

internal static class Crates
{
    public static IEnumerable<string> WhileRowNotEmpty(string row)
    {
        var copy = row;

        while (copy.Length > 0)
        {
            var crate = copy.Substring(0, 3);
            copy = copy.Length >= 4 ? copy.Substring(4) : string.Empty;
            yield return crate;
        }
    }
}

internal class Operator
{
    private readonly List<CargoStack> _stacks;
    private readonly List<Instruction> _instructions;

    public Operator(List<CargoStack> stacks, List<Instruction> instructions)
    {
        _stacks = stacks;
        _instructions = instructions;
    }

    public List<CargoStack> Organize(bool maintainOrder)
    {
        var copy = _stacks.Select(s => s.Clone()).ToList();
        foreach (var instruction in _instructions)
        {
            instruction.Execute(copy, maintainOrder);
        }
        return copy;
    }
    public string GetTopItems(List<CargoStack> stacks) => stacks.Aggregate(string.Empty, (result, stack) => result += stack.Pop().ToString());

}


public class DayFive
{
    public static void Run()
    {
        Console.WriteLine("******\nDay 5");
        var lines = File.ReadAllLines("Cinco/Input.txt");
        var stacks = lines.TakeWhile(s => s.StartsWith("["));
        var numberofColumns = int.Parse(lines.SkipWhile(s => !s.Trim().StartsWith("1")).First().Trim().Last().ToString());
        var moves = lines.SkipWhile(s => !s.StartsWith("move"));
        var instructions = moves.Select(Instruction.Create).ToList();
        var cargoStacks = Enumerable.Range(0,numberofColumns).Select(s => new CargoStack(s)).ToList();
        var index = 0;

        foreach (var section in stacks.Reverse())
        {
            foreach (var crate in Crates.WhileRowNotEmpty(section))
            {
                cargoStacks[index].AddCrate(Crate.Create(crate));
                index++;
            }
            index = 0;
        }

        var craneOperator = new Operator(cargoStacks, instructions);
        var topItems = craneOperator.GetTopItems(craneOperator.Organize(false));
        var newItems = craneOperator.GetTopItems(craneOperator.Organize(true));
        Console.WriteLine($"Top 9000 Items: {topItems}");
        Console.WriteLine($"Top 9001 Items: {newItems}");
    }
}