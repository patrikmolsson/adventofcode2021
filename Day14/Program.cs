// See https://aka.ms/new-console-template for more information


var input = File.ReadAllLines("14/input.txt");
// var input = File.ReadAllLines("14/test.txt");


var instructions = new Dictionary<char, IDictionary<char, char>>();
var topLevelPolymer = input[0].ToCharArray();

foreach (var instruction in input[2..])
{
    var spl = instruction.Split("->", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
    var parts = spl[0].ToCharArray();

    instructions.TryAdd(parts[0], new Dictionary<char, char>());
    instructions[parts[0]][parts[1]] = spl[1].ToCharArray().Single();
}

Console.WriteLine("Part ONE");
Process(10);

Console.WriteLine("Part TWO");
Process(40);

void Process(int untilDepth)
{
    var polymers = new List<Polymer>();
    var cache = new Dictionary<CacheKey, IDictionary<char, long>>();
    var counts = new Dictionary<char, long> { { topLevelPolymer[^1], 1 } };

    for (var i = 1; i < topLevelPolymer.Length; i++)
    {
        polymers.Add(new Polymer(topLevelPolymer[i - 1], topLevelPolymer[i], instructions, 0, cache));
    }

    foreach (var polymer in polymers)
    {
        foreach (var (key, value) in polymer.CountsUntilDepth(untilDepth))
        {
            if (counts.TryAdd(key, value))
            {
                continue;
            }

            counts[key] += value;
        }
    }


    var maxCount = counts.Values.Max();
    var minCount = counts.Values.Min();

    Console.WriteLine(maxCount - minCount);
}

internal record CacheKey(char PartOne, char PartTwo, int Depth);

internal class Polymer
{
    private readonly IDictionary<CacheKey, IDictionary<char, long>> cache;
    private readonly int depth;
    private readonly IDictionary<char, IDictionary<char, char>> instructions;
    private readonly char partOne;
    private readonly char partTwo;

    public Polymer(char partOne, char partTwo, IDictionary<char, IDictionary<char, char>> instructions, int depth,
        IDictionary<CacheKey, IDictionary<char, long>> cache)
    {
        this.partOne = partOne;
        this.partTwo = partTwo;
        this.instructions = instructions;
        this.depth = depth;
        this.cache = cache;
    }


    public IDictionary<char, long> CountsUntilDepth(int maxDepth)
    {
        if (maxDepth == this.depth)
        {
            return new Dictionary<char, long> { { this.partOne, 1 } };
        }

        var cacheKey = new CacheKey(this.partOne, this.partTwo, this.depth);
        if (this.cache.TryGetValue(cacheKey, out var prevCounts))
        {
            return prevCounts.ToDictionary(s => s.Key, s => s.Value);
        }

        var children = this.CraftPolymers().ToArray();

        var counts = children[0].CountsUntilDepth(maxDepth);

        foreach (var (key, value) in children[1].CountsUntilDepth(maxDepth))
        {
            if (counts.TryAdd(key, value))
            {
                continue;
            }

            counts[key] += value;
        }

        this.cache[cacheKey] = counts.ToDictionary(s => s.Key, s => s.Value);

        return counts;
    }

    private IEnumerable<Polymer> CraftPolymers()
    {
        yield return new Polymer(this.partOne, this.instructions[this.partOne][this.partTwo], this.instructions,
            this.depth + 1, this.cache);
        yield return new Polymer(this.instructions[this.partOne][this.partTwo], this.partTwo, this.instructions,
            this.depth + 1, this.cache);
    }
}
