namespace AdventCode.Seis
{
    public class DaySix
    {
        public static void Run()
        {
            Console.WriteLine("******");
            Console.WriteLine("Day 6");
            var lines = File.ReadAllLines("Seis/Input.txt");
            Console.WriteLine($"Markers: {string.Join(" ", FindMarkers(lines, 4))}");
            Console.WriteLine($"Markers: {string.Join(" ", FindMarkers(lines, 14))}");
        }

        private static IEnumerable<int> FindMarkers(IEnumerable<string> lines, int takeAmount)
        {
            var markers = lines.Select(line =>
            {
                for (var i = 0; i < line.Length; i++)
                {
                    var duplicateFound = line.Skip(i)
                        .Take(takeAmount)
                        .GroupBy(letter => letter)
                        .Any(group => group.Count() > 1);
                    if (duplicateFound)
                        continue;
                    return i + takeAmount;
                }
                return 0;
            })
                .ToList();
            return markers;
        }
    }

}
