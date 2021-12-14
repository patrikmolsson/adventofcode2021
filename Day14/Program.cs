// See https://aka.ms/new-console-template for more information


using System.Text;

// var input = File.ReadAllLines("14/input.txt");
var input = File.ReadAllLines("14/test.txt");

var polymer = input[0].ToCharArray();

var instructions = new Dictionary<char, IDictionary<char, char>>();

foreach (var instruction in input[2..])
{
    var spl = instruction.Split("->", StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
    var parts = spl[0].ToCharArray();

    instructions.TryAdd(parts[0], new Dictionary<char, char>());
    instructions[parts[0]][parts[1]] = spl[1].ToCharArray().Single();
}


// for (var iteration = 1; iteration <= 10; iteration++)
// {
//     polymer = Process(polymer, instructions).ToArray();
//
//     Console.WriteLine(iteration);
// }
//
// var counts = new Dictionary<char, long>();
//
// foreach (var c in polymer)
// {
//     counts.TryAdd(c, 0);
//     counts[c] += 1;
// }
//
//
// var mostCommon = counts.MaxBy(s => s.Value);
// var leastCommon = counts.MinBy(s => s.Value);
//
// Console.WriteLine($"Most common {mostCommon.Key}:{mostCommon.Value}");
// Console.WriteLine($"Least common {leastCommon.Key}:{leastCommon.Value}");
// Console.WriteLine($"Answer: {mostCommon.Value - leastCommon.Value}");

var polymers = new List<Polymer>();

for (var i = 1; i < polymer.Length; i++)
{
    polymers.Add(new Polymer(polymer[i - 1], polymer[i], instructions, 0));
}

// var counts = new Dictionary<char, long>()
// {
//     {polymer[^1], 1}
// };
var counts = new Dictionary<char, long>()
{
};

for (var i = 0; i <= 5; i++)
{
    Console.WriteLine($"Depth: {i}");
    foreach (var (key, value) in polymers[0].CountsUntilDepth(i))
    {
        Console.WriteLine($"\t: {key}:{(key == 'N' ? (value + 1).ToString() : value.ToString())}");
    }
}
//
// foreach (var polymer1 in polymers)
// {
//     foreach (var countsPolymer in polymer1.CountsUntilDepth(41))
//     {
//         if (counts.TryAdd(countsPolymer.Key, countsPolymer.Value))
//         {
//             continue;
//         }
//
//         counts[countsPolymer.Key] += countsPolymer.Value;
//     }
//     Console.WriteLine("Processed polymer");
// }
//
// Console.WriteLine(counts.Count);
//
// var maxCount = counts.Values.Max();
// var minCount = counts.Values.Min();
//
// Console.WriteLine(maxCount - minCount);
//
//

static IEnumerable<char> Process(IReadOnlyList<char> template, IDictionary<char, IDictionary<char,char>> instructions)
{
    yield return template[0];

    for (var i = 1; i < template.Count; i++)
    {
        var t1 = template[i - 1];
        var t2 = template[i];

        var newChar = instructions[t1][t2];

        yield return newChar;
        yield return t2;
    }
}

record CacheKey(char PartOne, char PartTwo, int Depth);

class Polymer
{
    private readonly char partOne;
    private readonly char partTwo;
    private readonly IDictionary<char, IDictionary<char, char>> instructions;
    private readonly int depth;

    public Polymer(char partOne, char partTwo, IDictionary<char, IDictionary<char, char>> instructions, int depth)
    {
        this.partOne = partOne;
        this.partTwo = partTwo;
        this.instructions = instructions;
        this.depth = depth;
    }


    public IDictionary<char, long> CountsUntilDepth(int maxDepth)
    {
        if (maxDepth == this.depth)
        {
            return new Dictionary<char, long>() {{this.partOne, 1}};
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

        return counts;
    }

    private IEnumerable<Polymer> CraftPolymers()
    {
        yield return new Polymer(this.partOne, this.instructions[this.partOne][this.partTwo], this.instructions,
            this.depth + 1);
        yield return new Polymer(this.instructions[this.partOne][this.partTwo], this.partTwo, this.instructions,
            this.depth + 1);
    }

}
